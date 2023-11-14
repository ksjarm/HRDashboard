<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
    <div class="text-center">
      <h1 class="font-bold schedule-header">{{ title }}</h1>
    </div>
    <button @click="openShiftModal" class="btn-create-shift">Create New Shift</button>

    <div v-if="shiftModalVisible" class="shift-modal">
      <h2>Create New Shift</h2>
      <input v-model="newShift.title" placeholder="Title" />

      <label>
        <input v-model="newShift.valik" type="radio" value="Onetime" />
        One-Time Shift
      </label>

      <label>
        <input v-model="newShift.valik" type="radio" value="Recurring" />
        Recurring Shift
      </label>

      <div v-if="newShift.valik === 'Recurring'">
        <label>Start Date:</label>
        <input v-model="newShift.startDate" type="date" />
        <label>End Date:</label>
        <input v-model="newShift.endDate" type="date" />

        <!-- Dropdown list for selecting a single weekday -->
        <label>Select Weekday:</label>
        <select v-model="newShift.selectedWeekDay">
          <option v-for="day in weekdays" :key="day" :value="day">{{ day }}</option>
        </select>
      </div>

      <div v-if="newShift.valik === 'Onetime'">
        <label>Date:</label>
        <input v-model="newShift.date" type="date" />
      </div>

      <label>Shift Time:</label>
      <input v-model="newShift.startTime" type="time" placeholder="Start Time" />
      <input v-model="newShift.endTime" type="time" placeholder="End Time" />

      <button
        @click="addNewShift"
        class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
      >
        <span class="absolute left-0 inset-y-0 flex items-center pl-3"></span>
        Add shift
      </button>
    </div>
  
    <FullCalendar
      class="demo-app-calendar"
      :options="calendarOptions"
      :events="calendarOptions.events"
    >
      <template v-slot:eventContent="arg">
        <b>{{ arg.timeText }}</b>
        <i>{{ arg.event.title }}</i>
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
      title, // <-- Explicitly typing the title parameter
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

  resetNewShiftForm();
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
  // Use await to ensure the asynchronous operation completes before proceeding
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
  // Increment the counter and return the new ID as a number
  return ++counter;
}

// Helper function to convert date format to ISO 8601
function formatToISODate(dateString: string): string {
  const parts = dateString.split('-');
  if (parts.length === 3) {
    return `${parts[0]}-${parts[1]}-${parts[2]}`;
  }
  return dateString; // Return as is if invalid format
}

// Helper function to convert time format to ISO 8601
function formatTime(timeString: string): string {
  const parts = timeString.split(':');
  if (parts.length === 2) {
    return `${parts[0]}:${parts[1]}`;
  }
  return timeString; // Return as is if invalid format
}
const handleEventClick = (arg: any) => {
  console.log('Event clicked:', arg.event.title, 'ID:', arg.event.id);

  const confirmed = window.confirm(`Are you sure you want to delete the shift '${arg.event.title}'?`);

  if (confirmed) {
    // Convert the FullCalendar event ID to a number
    const eventId = parseInt(arg.event.id);

    const shift = shifts.value.find((s) => s.id === eventId);

    if (shift) {
      console.log('Removing shift:', shift);
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
  events: [] as any[], // Explicitly set the type here as any[]
  eventClick: handleEventClick,
});

onMounted(() => {
  shiftsStore.load();
  updateCalendarEvents();
});

const updateCalendarEvents = () => {
  calendarOptions.value.events = shifts.value.map((shift) => ({
    id: shift.id, // Set the event's id to the shift's id
    title: shift.title,
    start: `${shift.date}T${shift.startTime}`,
    end: `${shift.date}T${shift.endTime}`,
  }));
};


const openShiftModal = () => {
  shiftModalVisible.value = true;
};

watch(shifts, () => {
  updateCalendarEvents();
});
</script>

<style scoped>
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
  margin-top: 20px; /* Adjust margin as needed */
  /* Add any additional CSS styles for the calendar container */
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
  margin-bottom: 10px; /* Adjust the margin to control the spacing */
}
</style>
