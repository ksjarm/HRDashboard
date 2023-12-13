<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
    <div class="text-center">
      <h1 class="font-bold schedule-header">{{ title }}</h1>
    </div>
    <div>
      <button @click="toggleWeekends">Toggle Weekends</button>
    </div>

    <button @click="openShiftModal" class="btn-create-shift">
      Create New Shift
    </button>

    <div v-if="confirmationModalVisible" class="modal-overlay">
      <div class="modal">
        <div class="modal-header">
          <h2>Confirm Action</h2>
          <button @click="closeConfirmationModal" class="close-button">
            Close
          </button>
        </div>
        <p>Do you want to delete or edit the shift?</p>
        <div class="button-container">
          <button @click="deleteShift" class="btn-delete">Delete Shift</button>
          <button @click="editShift" class="btn-edit">Edit Shift</button>
        </div>
      </div>
    </div>

    <div
      v-if="shiftModalVisible || editShiftModalVisible"
      class="modal-overlay"
    >
      <div class="modal">
        <div class="modal-header">
          <h2>
            {{ editShiftModalVisible ? 'Edit Shift' : 'Create New Shift' }}
          </h2>
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
        <label>Select Employee:</label>
<select v-model="selectedEmployeeId" class="full-width">
  <option v-for="employee in employees" :key="employee.id" :value="employee.id">
    {{ employee.name }}
  </option>
</select>

        <div v-if="newShift.valik === 'Recurring'">
          <label>Start Date:</label>
          <input v-model="newShift.startDate" type="date" />
          <label>End Date:</label>
          <input v-model="newShift.endDate" type="date" />

          <label class="weekday-label">Select Weekday:</label>
          <div class="weekday-input">
            <select v-model="newShift.selectedWeekDay" class="full-width">
              <option v-for="day in weekdays" :key="day" :value="day">
                {{ day }}
              </option>
            </select>
          </div>
        </div>

        <div v-if="newShift.valik === 'Onetime'">
          <label>Date:</label>
          <input v-model="newShift.date" type="date" />
        </div>

        <label>Shift Time:</label>
        <input
          v-model="newShift.startTime"
          type="time"
          placeholder="Start Time"
        />
        <input v-model="newShift.endTime" type="time" placeholder="End Time" />
        <div v-if="validationError" class="validation-error">
          {{ validationError }}
        </div>
        <button
          @click="validateAndAddOrEditShift"
          class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          <span class="absolute left-0 inset-y-0 flex items-center pl-3"></span>
          {{ editShiftModalVisible ? 'Save changes' : 'Add Shift' }}
        </button>
      </div>
    </div>

    <FullCalendar
      class="demo-app-calendar"
      :options="calendarOptions"
      :events="calendarOptions.events"
    >
    <template v-slot:eventContent="arg">
  <div class="custom-event-content" @click="handleEventClick(arg)">
    <b v-if="!shiftModalVisible">
      {{ arg.event.title.split('\n')[0] }} - <br />
       {{ arg.event.title.split('\n')[1] }}
    </b>
    <b v-else>{{ arg.event.title }}</b>
  </div>
</template>

    </FullCalendar>
  </div>
</template>

<script setup lang="ts">
import { useShiftsStore } from '@/stores/shiftsStore';
import { useEmployeesStore } from '@/stores/employeesStore';
import { onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import FullCalendar from '@fullcalendar/vue3';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Shift } from '@/modules/shift';
import { watch } from 'vue';
import { useAuthStore } from '@/stores/authStore';
import { useRouter } from 'vue-router';

const router = useRouter();

const auth = useAuthStore();
const { isAuthenticated } = storeToRefs(auth);

const shiftsStore = useShiftsStore();
const { shifts} = storeToRefs(shiftsStore);


const employeesStore = useEmployeesStore();
const { employees} = storeToRefs(employeesStore);

defineProps<{ title: String }>();

const shiftModalVisible = ref(false);
const newShift = ref({
  title: '',
  date: new Date().toISOString().slice(0, 10),
  startTime: '',
  endTime: '',
  valik: 'Onetime', 
  startDate: new Date().toISOString().slice(0, 10),
  endDate: new Date().toISOString().slice(0, 10),
  selectedWeekDay: '',
  employeeIds: [],
});
const confirmationModalVisible = ref(false);
let selectedShift: Shift | null = null;
const editShiftModalVisible = ref(false);

const selectedEmployeeId = ref(null);

const editShift = () => {
  console.log('Edit Shift Clicked');
  console.log('Selected Shift:', selectedShift);
  openEditShiftModal();
  closeConfirmationModal();
};

const openEditShiftModal = () => {
  if (selectedShift) {
    newShift.value.title = selectedShift.title;
    newShift.value.date = selectedShift.date || '';
    newShift.value.startTime = selectedShift.startTime;
    newShift.value.endTime = selectedShift.endTime;
    newShift.value.valik = selectedShift.valik;
    newShift.value.startDate = selectedShift.startDate || '';
    newShift.value.endDate = selectedShift.endDate || '';
    newShift.value.selectedWeekDay = selectedShift.selectedWeekDay || '';

    editShiftModalVisible.value = true;
    document.body.style.overflow = 'hidden';
  }
};

const validateAndAddOrEditShift = async () => {
  validationError.value = '';

  if (!validateInput()) {
    return;
  }

  if (editShiftModalVisible.value && selectedShift) {
    shiftsStore.updateShift(selectedShift);
    closeShiftModal();
  } else {
    if (selectedEmployeeId.value) {
      newShift.value.employeeIds = [selectedEmployeeId.value];
    }

    if (newShift.value.valik === 'Onetime') {

      addNewShift();
    } else if (newShift.value.valik === 'Recurring') {

      if (newShift.value.selectedWeekDay && newShift.value.startDate && newShift.value.endDate) {
        const selectedWeekdays = Array.isArray(newShift.value.selectedWeekDay)
          ? newShift.value.selectedWeekDay
          : [newShift.value.selectedWeekDay];
        const recurringShifts = generateRecurringShifts(
          newShift.value.title,
          newShift.value.startDate,
          newShift.value.endDate,
          selectedWeekdays,
          newShift.value.startTime,
          newShift.value.endTime,
        );

        for (const shift of recurringShifts) {
          await addAndDisplayShift(shift);
        }
        closeShiftModal();
        resetNewShiftForm();
      } else {
        validationError.value = 'Please select the week day, start date, and end date for recurring shifts.';
      }
    }
  }
};

const handleEventClick = (arg: any) => {
  selectedShift =
    shifts.value.find((s) => s.id === parseInt(arg.event.id)) || null;
  openConfirmationModal();
};

const openConfirmationModal = () => {
  confirmationModalVisible.value = true;
  document.body.style.overflow = 'hidden';
};

const closeConfirmationModal = () => {
  confirmationModalVisible.value = false;
  document.body.style.overflow = '';
};

const deleteShift = () => {
  if (selectedShift) {
    removeShift(selectedShift);
    closeConfirmationModal();
  }
};

const handleDateClick = (arg: any) => {
  openShiftModal();

  const dateStr = formatToISODate(arg.date);
  const timeStr = formatToISOTime(arg.date); 
  newShift.value.date = dateStr;
  newShift.value.startDate = dateStr;
  newShift.value.endDate = dateStr;
  newShift.value.valik = 'Onetime';
  newShift.value.startTime = timeStr;
};
const formatToISOTime = (dateString: string): string => {
  const date = new Date(dateString);
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  return `${hours}:${minutes}`;
};
const weekdays = [
  'Sunday',
  'Monday',
  'Tuesday',
  'Wednesday',
  'Thursday',
  'Friday',
  'Saturday',
];

const validationError = ref('');

const validateInput = () => {
  if (
    !newShift.value.title ||
    !newShift.value.startTime ||
    !newShift.value.endTime
  ) {
    validationError.value = 'Please fill in all required fields.';
    return false;
  }
  return true;
};
const addNewShift = async () => {
  const {
    title,
    valik,
    startDate,
    endDate,
    selectedWeekDay,
    startTime,
    endTime,
  } = newShift.value;

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
    const selectedWeekdays = Array.isArray(selectedWeekDay)
      ? selectedWeekDay
      : [selectedWeekDay];
    const recurringShifts = generateRecurringShifts(
      title,
      startDate,
      endDate,
      selectedWeekdays,
      startTime,
      endTime,
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
  endTime: string,
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
  shift.employeeIds = newShift.value.employeeIds;
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
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function formatTime(timeString: string): string {
  const parts = timeString.split(':');
  if (parts.length === 2) {
    return `${parts[0]}:${parts[1]}`;
  }
  return timeString;
}
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
  dateClick: handleDateClick,
});

onMounted(() => {
  if (!isAuthenticated.value) {
    router.push({ name: 'Log in' });
  }
  shiftsStore.load();
  updateCalendarEvents();
});


const updateCalendarEvents = () => {
  calendarOptions.value.events = shifts.value.map((shift) => ({
    id: shift.id,
    title: `${shift.title}\nAssigned to: ${getEmployeeNames(shift.employeeIds) || 'None'}`,
    start: `${shift.date}T${shift.startTime}`,
    end: `${shift.date}T${shift.endTime}`,
  }));
};

const getEmployeeNames = (employeeIds: any): string => {
  if (!Array.isArray(employeeIds)) {
    return 'None';
  }

  return employeeIds
    .map((employeeId) => {
      const employee = employees.value.find((emp) => emp.id === employeeId);
      return employee ? employee.name : 'None';
    })
    .join(', ');
};

const openShiftModal = (
  payload?: MouseEvent,
  defaultDate: string | null = null,
) => {
  if (payload) {
    payload.preventDefault();
  }
  shiftModalVisible.value = true;
  document.body.style.overflow = 'hidden';

  resetNewShiftForm();

  newShift.value.date = defaultDate || new Date().toISOString().slice(0, 10);
  newShift.value.startDate =
    defaultDate || new Date().toISOString().slice(0, 10);
  newShift.value.endDate = defaultDate || new Date().toISOString().slice(0, 10);
  newShift.value.valik = 'Onetime';
};

const closeShiftModal = () => {
  if (editShiftModalVisible.value) {
    editShiftModalVisible.value = false;
  } else {
    shiftModalVisible.value = false;
    validationError.value = '';
    selectedEmployeeId.value = null;
    document.body.style.overflow = '';
  }
};
watch(shifts, () => {
  updateCalendarEvents();
});
</script>

<style scoped>
.custom-event-content {
  max-width: 200px; 
  max-height: 100px; 
  overflow: hidden;
  text-overflow: ellipsis; 
  white-space: nowrap; 
}
.event-title {
  font-weight: bold;
}
.assigned-employees {
  margin-top: 5px;
  font-size: 0.9em;
  color: #555; 
}
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
  backdrop-filter: blur(5px); 
  z-index: 2; 
  pointer-events: auto; 
}
.validation-error {
  color: red;
  margin-bottom: 10px;
}
.modal-header {
  position: relative;
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
  filter: blur(5px);
}
.modal {
  max-height: 80vh; 
  overflow-y: auto; 
  background: #f8f8f8; 
  padding: 15px;
  border-radius: 12px; 
  width: 500px;
  z-index: 3; 
  display: flex;
  flex-direction: column;
  gap: 15px; 
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1); 
}
.close-button {
  position: absolute;
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  color: #333;
}
.close-button:hover {
  color: #ff0000; 
}
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
  padding: 12px;
  background-color: #f0f0f0; 
  border: none; 
  border-radius: 8px; 
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
  margin-top: 10px; 
}
.weekday-input {
  margin-top: 5px; 
}
.full-width {
  width: 100%;
  padding: 6px; 
  border-radius: 8px; 
  background-color: #f0f0f0; 
  border: 1px solid #ccc; 
}
button-container {
  display: flex;
  justify-content: center;
}
.btn-delete,
.btn-edit {
  background-color: #e53e3e;
  color: white;
  padding: 10px;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  font-weight: bold;
  margin-right: 10px;
}
.btn-edit {
  background-color: #4299e1;
}
</style>