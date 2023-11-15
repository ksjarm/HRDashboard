<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
    <div class="text-center">
      <h1 class="font-bold schedule-header">{{ title }}</h1>
    </div>
    <div>
      <button @click="toggleWeekends">Toggle Weekends</button>
    </div>
   
    <button @click="openShiftModal" class="btn-create-shift">Create New Shift</button>

    <div v-if="shiftModalVisible" class="modal-overlay">
      <div class="modal">
        <div class="modal-header">
        <h2>Create New Shift</h2>
      <button @click="closeShiftModal" class="close-button">Close</button>
      </div>
      <input v-model="newShift.title" placeholder="Title" maxlength="50" />

      <label class="shift-type-label">
        <input v-model="newShift.valik" type="radio" value="Onetime" />
        <span>One-Time Shift</span>
      </label>

      <label class="shift-type-label">
        <input v-model="newShift.valik" type="radio" value="Recurring" />
        <span>Recurring Shift</span>
      </label>

      <div v-if="newShift.valik === 'Recurring'">
        <label>Start Date:</label>
        <input v-model="newShift.startDate" type="date" />
        <label>End Date:</label>
        <input v-model="newShift.endDate" type="date" />

        <!-- Dropdown list for selecting a single weekday -->
        <label class="weekday-label">Select Weekday:</label>
        <div class="weekday-input">
        <select v-model="newShift.selectedWeekDay" class="full-width">
          <option v-for="day in weekdays" :key="day" :value="day">{{ day }}</option>
        </select>
        </div>
      </div>

      <div v-if="newShift.valik === 'Onetime'">
        <label>Date:</label>
        <input v-model="newShift.date" type="date" />
      </div>

      <label>Shift Time:</label>
      <input v-model="newShift.startTime" type="time" placeholder="Start Time" />
      <input v-model="newShift.endTime" type="time" placeholder="End Time" />
      <div v-if="validationError" class="validation-error">{{ validationError }}</div>
      <button
      @click="validateAndAddShift"
        class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
      >
        <span class="absolute left-0 inset-y-0 flex items-center pl-3"></span>
        Add shift
      </button>
      </div>
    </div>
  
    <FullCalendar
      class="demo-app-calendar"
      :options="calendarOptions"
      :events="calendarOptions.events"
    >
      <template v-slot:eventContent="arg">
        <b v-if="!shiftModalVisible" @click="handleEventClick(arg)">{{ arg.event.title }}</b>
       <b v-else>{{ arg.event.title }}</b>
      </template>
    </FullCalendar>
  </div>
</template>

<script setup lang="ts">
import { useShiftsStore } from '@/stores/shiftsStore';
import { onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import FullCalendar from '@fullcalendar/vue3';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Shift } from '@/modules/shift';
import { watch } from 'vue';

const shiftsStore = useShiftsStore();
const { shifts } = storeToRefs(shiftsStore);

defineProps<{ title: String }>();

const shiftModalVisible = ref(false);
const newShift = ref({
  title: '',
  date: new Date().toISOString().slice(0, 10),
  startTime: '',
  endTime: '',
  valik: 'Onetime', // Default type is 'Onetime'
  startDate: new Date().toISOString().slice(0, 10),
  endDate: new Date().toISOString().slice(0, 10),
  selectedWeekDay: '',
});

const weekdays = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

const validationError = ref('');

const validateAndAddShift = async () => {
  // Reset previous validation error
  validationError.value = '';

  // Validate input fields
  if (!validateInput()) {
    return;
  }

  // Continue with adding a new shift
  addNewShift();
};

const validateInput = () => {
  if (!newShift.value.title || !newShift.value.startTime || !newShift.value.endTime) {
    validationError.value = 'Please fill in all required fields.';
    return false;
  }

  return true;
};
const addNewShift = async () => {
  const { title, valik, startDate, endDate, selectedWeekDay, startTime, endTime } = newShift.value;

  if (valik === 'Onetime') {
    const newShiftData = {
      id: generateUniqueId(),
      title,
      date: formatToISODate(newShift.value.date),
      startTime: formatTime(startTime),
      endTime: formatTime(endTime),
      valik: 'Onetime',
    };

    await addAndDisplayShift(newShiftData);
  } else {
    const selectedWeekdays = Array.isArray(selectedWeekDay) ? selectedWeekDay : [selectedWeekDay];
    const recurringShifts = generateRecurringShifts(
      title, 
      startDate,
      endDate,
      selectedWeekdays,
      startTime,
      endTime
    );

    for (const shift of recurringShifts) {
      await addAndDisplayShift(shift);
    }
  }
  closeShiftModal();
  resetNewShiftForm();
};
const toggleWeekends = () => {
  calendarOptions.value.weekends = !calendarOptions.value.weekends;
  updateCalendarEvents();
};
const generateRecurringShifts = (
  title: string, 
  startDate: string,
  endDate: string,
  selectedWeekdays: string[],
  startTime: string,
  endTime: string
) => {
  const shifts = [];
  const currentDate = new Date(startDate);
  const end = new Date(endDate);

  while (currentDate <= end) {
    if (selectedWeekdays.includes(weekdays[currentDate.getDay()])) {
      shifts.push({
        id: generateUniqueId(),
        title,
        date: formatToISODate(currentDate.toISOString().slice(0, 10)),
        startTime: formatTime(startTime),
        endTime: formatTime(endTime),
        valik: 'Recurring',
      });
    }

    currentDate.setDate(currentDate.getDate() + 1);
  }

  return shifts;
};
const addAndDisplayShift = async (shift: Shift) => {
  while (shifts.value.some((existingShift) => existingShift.id === shift.id)) {
    shift.id = generateUniqueId();
  }

  await shiftsStore.addShift(shift);
  updateCalendarEvents();
};
const removeShift = async (shift: Shift) => {
  await shiftsStore.deleteShift(shift);
  updateCalendarEvents();
};

const resetNewShiftForm = () => {
  newShift.value.title = '';
  newShift.value.date = new Date().toISOString().substr(0, 10);
  newShift.value.startTime = '';
  newShift.value.endTime = '';
  newShift.value.valik = 'Onetime'; // Default type is 'Onetime'
  newShift.value.startDate = new Date().toISOString().substr(0, 10);
  newShift.value.endDate = new Date().toISOString().substr(0, 10);
  newShift.value.selectedWeekDay = '';
};

let counter = 0;

function generateUniqueId() {
  return ++counter;
}

function formatToISODate(dateString: string): string {
  const parts = dateString.split('-');
  if (parts.length === 3) {
    return `${parts[0]}-${parts[1]}-${parts[2]}`;
  }
  return dateString; 
}

function formatTime(timeString: string): string {
  const parts = timeString.split(':');
  if (parts.length === 2) {
    return `${parts[0]}:${parts[1]}`;
  }
  return timeString; 
}
const handleEventClick = (arg: any) => {

  const confirmed = window.confirm(`Are you sure you want to delete the shift '${arg.event.title}'?`);

  if (confirmed) {
    const shift = shifts.value.find((s) => s.id === parseInt(arg.event.id));

    if (shift) {
      removeShift(shift);
    } else {
      console.log('Shift not found');
    }
  }
};
const calendarOptions = ref({
  plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
  headerToolbar: {
    left: 'prev,next today',
    center: 'title',
    right: 'dayGridMonth,timeGridWeek,timeGridDay',
  },
  initialView: 'dayGridMonth',
  weekends: false,
  events: [] as any[],
  eventClick: handleEventClick,
  
});

onMounted(() => {
  shiftsStore.load();
  updateCalendarEvents();
});

const updateCalendarEvents = () => {
  calendarOptions.value.events = shifts.value.map((shift) => ({
    id: shift.id, 
    title: shift.title,
    start: `${shift.date}T${shift.startTime}`,
    end: `${shift.date}T${shift.endTime}`,
  }));
};


const openShiftModal = () => {
  shiftModalVisible.value = true;
  document.body.style.overflow = 'hidden'; // Prevent scrolling when modal is open
};

const closeShiftModal = () => {
  shiftModalVisible.value = false;
  validationError.value = '';
  document.body.style.overflow = ''; // Reset overflow to default
};


watch(shifts, () => {
  updateCalendarEvents();
});
</script>

<style scoped>
/* ... your existing styles ... */
.validation-error {
  color: red;
  margin-bottom: 10px;
}
.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 10px;
  border-bottom: 1px solid #ccc;
  margin-bottom: 10px;
}

.modal-header h2 {
  margin: 0;
}
.blurred {
  filter: blur(5px); /* Adjust the blur level as needed */
}

/* Add a style to prevent pointer events when the shift modal is open */

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  backdrop-filter: blur(5px); /* Add backdrop-filter for blur effect */
  z-index: 2; /* Ensure the modal overlay is above the underlying content */
  pointer-events: auto; /* Enable pointer events on the overlay */
}

.modal {
    background: #f8f8f8; /* Light gray background */
    padding: 15px;
    border-radius: 12px; /* More rounded corners */
    width: 500px;
    z-index: 3; /* Ensure the modal is above the modal overlay */
    display: flex;
    flex-direction: column;
    gap: 15px; /* Increased space between positions */
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1); /* Optional: Add a subtle box shadow */
  }

.close-button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  color: #333;
}

.close-button:hover {
  color: #ff0000; /* Change color on hover if desired */
}

/* Ensure the FullCalendar is below the modal and modal overlay */
.demo-app-calendar {
  position: relative;
  z-index: 1;
}
.schedule-header {
  font-size: 24px;
  color: #333;
  text-align: center;
}

.btn-create-shift {
  margin-top: 10px;
  background-color: rgb(76, 99, 175);
  color: #fff;
  padding: 10px 20px;
  border: none;
  cursor: pointer;
}
.shift-modal {
  margin: 20px;
  padding: 20px;
  border: 1px solid #ccc;
  background-color: #fff;
}
.calendar-container {
  margin-top: 20px; 
}
.btn-remove-shift {
  background-color: red;
  color: white;
  padding: 5px 10px;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  font-weight: bold;
}

.event-list {
  list-style-type: none;
  padding: 0;
}

.event-list-item {
  margin-bottom: 10px; 
}
input {
    padding: 12px; /* Increased padding */
    background-color: #f0f0f0; /* Lighter gray background for input fields */
    border: none; /* Remove the border */
    border-radius: 8px; /* More rounded corners */
    width: 100%;
    box-sizing: border-box;
  }

  label {
    font-weight: bold;
    margin-bottom: 5px;
  }

  .radio-label {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
  }

  .radio-label input {
    margin-right: 5px;
  }
  .shift-type-label {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
  }

  .shift-type-label input {
    vertical-align: middle;
    margin-right: -450px;
  }

  .shift-type-label span {
    vertical-align: middle;
  }
  .weekday-label {
    margin-top: 10px; /* Add space between the select weekday and the labels above */
  }
  .weekday-input {
    margin-top: 5px; /* Adjust the margin as needed */
  }
  .full-width {
    width: 100%; /* Adjust the width and consider any padding or margin */
    padding: 6px; /* Add padding to match the input fields */
    border-radius: 8px; /* Add border-radius for rounded corners */
    background-color: #f0f0f0; /* Lighter gray background for consistency */
    border: 1px solid #ccc; /* Remove the default border */
  }

  
</style>
