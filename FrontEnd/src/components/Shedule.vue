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
      <button
            @click="addNewShift"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          >
            <span class="absolute left-0 inset-y-0 flex items-center pl-3">
            </span>
            Add shift
          </button>
        </div>

        <div>
        <ul class="event-list">
          <li v-for="shift in shifts" :key="shift.id" class="event-list-item" >
            {{ shift.title }} - {{ shift.date }} ({{ shift.startTime }} - {{ shift.endTime }})
            <button @click="removeShift(shift)" class="btn-remove-shift">Remove Shift</button>
          </li>
        </ul>
      </div>
    <FullCalendar
      class='demo-app-calendar'
      :options='calendarOptions'
      :events='calendarOptions.events'
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
});


const addNewShift = async () => {
  const newShiftData: Shift = {
    id: generateUniqueId(),
    title: newShift.value.title,
    date: formatToISODate(newShift.value.date),
    startTime: formatTime(newShift.value.startTime),
    endTime: formatTime(newShift.value.endTime),
    // Add other properties for shift details as needed
  };

  while (shifts.value.some((shift) => shift.id === newShiftData.id)) {
    newShiftData.id = generateUniqueId();
  }

  // Use async/await to ensure the shift is added before proceeding
  await shiftsStore.addShift(newShiftData);

  // After adding the shift, update the UI and perform other operations
  updateCalendarEvents();
  shiftModalVisible.value = false;
  resetNewShiftForm();
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
});

onMounted(() => {
  shiftsStore.load();
  updateCalendarEvents();
});

const updateCalendarEvents = () => {
  calendarOptions.value.events = shifts.value.map((shift) => ({
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
