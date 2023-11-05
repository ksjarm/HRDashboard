<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
    <div class="text-center">
      <h1 class="font-bold schedule-header">{{ title }}</h1>
    </div>
    <button @click="openShiftModal" class="btn-create-shift">Create New Shift</button>

    <div v-if="shiftModalVisible" class="shift-modal">
      <h2>Create New Shift</h2>
      <input v-model="newShift.title" placeholder="Title" />
      <input v-model="newShift.date" type="date" placeholder="Date" />
      <input v-model="newShift.startTime" type="time" placeholder="Start Time" />
      <input v-model="newShift.endTime" type="time" placeholder="End Time" />
      <button @click="addNewShift">Add Shift</button>
    </div>

    <div>{{ shifts }}</div>
    <FullCalendar
      class='demo-app-calendar'
      :options='calendarOptions'
      :events='shifts'
    >
      <template v-slot:eventContent='arg'>
        <b>{{ arg.timeText }}</b>
        <i>{{ arg.event.title }}</i>
      </template>
    </FullCalendar>
  </div>
</template>

<script setup lang="ts">
import { useShiftsStore } from '@/stores/shiftsStore';
import { onMounted, ref} from 'vue';
import { storeToRefs } from 'pinia';
import FullCalendar from '@fullcalendar/vue3';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Shift } from '@/modules/shift';
const {  updateShift } = useShiftsStore();

const shiftsStore = useShiftsStore();
const { shifts } = storeToRefs(shiftsStore);
defineProps<{ title: String }>();

const shiftModalVisible = ref(false);
const newShift = ref({
  title: '',
  date: new Date().toISOString().substr(0, 10),
  startTime: '',
  endTime: '',
});

const addNewShift = () => {
  const newShiftData: Shift = {
    id: generateUniqueId(), // Generate a unique ID
    title: newShift.value.title,
    date: formatToISODate(newShift.value.date),
    startTime: formatTime(newShift.value.startTime),
    endTime: formatTime(newShift.value.endTime),
    // Add other properties for shift details as needed
  };

  // Check if the ID is already in use and generate a new ID if needed
  while (shifts.value.some((shift) => shift.id === newShiftData.id)) {
    newShiftData.id = generateUniqueId();
  }
  if (newShiftData.id === 0) {
    shiftsStore.addShift(newShiftData);
  shifts.value.push(newShiftData);
  } else {
    updateShift(newShiftData);
  }
  shiftModalVisible.value = false;
  resetNewShiftForm();
  
};

const resetNewShiftForm = () => {
  newShift.value.title = '';
  newShift.value.date = new Date().toISOString().substr(0, 10);
  newShift.value.startTime = '';
  newShift.value.endTime = '';
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

const calendarOptions = {
  plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
  headerToolbar: {
    left: 'prev,next today',
    center: 'title',
    right: 'dayGridMonth,timeGridWeek,timeGridDay',
  },
  initialView: 'dayGridMonth',
  weekends: false,
  events: shifts.value.map((shift) => ({
    title: shift.title,
    start: `${shift.date}T${shift.startTime}`,
    end: `${shift.date}T${shift.endTime}`,
  })),
};

onMounted(() => {
  shiftsStore.load();
});


const openShiftModal = () => {
  shiftModalVisible.value = true;
};
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
</style>
