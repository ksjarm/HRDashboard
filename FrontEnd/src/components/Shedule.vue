<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
    <div class="text-center">
      <h1 class="font-bold schedule-header">{{ title }}</h1>
    </div>

    <!-- Add the FullCalendar component here -->
    <FullCalendar :options="calendarOptions" class="calendar-container" />
  </div>
</template>

<script setup lang="ts">
import { useShiftsStore } from '@/stores/shiftsStore';
import { onMounted, ref, watch } from 'vue';
import FullCalendar from '@fullcalendar/vue3';
import dayGridPlugin from '@fullcalendar/daygrid';

defineProps<{ title: String }>();

const shiftsStore = useShiftsStore();
const shiftTitleFilter = ref<string>('');

// Define your FullCalendar options
const calendarOptions = ref({
  plugins: [dayGridPlugin],
  initialView: 'dayGridMonth',
  weekends: false,
  events: [
    { title: 'Meeting', start: new Date() }
    // Add more events as needed
  ],
});

onMounted(() => {
  shiftsStore.load();
});

watch(shiftTitleFilter, (title) => {
  shiftsStore.filterShiftsByTitle(title);
});
</script>

<style scoped>
.schedule-header {
  font-size: 24px;
  color: #333;
  text-align: center;
}

.calendar-container {
  margin-top: 20px; /* Adjust margin as needed */
  /* Add any additional CSS styles for the calendar container */
}
</style>
