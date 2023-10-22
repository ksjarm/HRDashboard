<template>
    <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
      <div class="text-center">
        <h1 class="font-bold">
          <span class="schedule-text">Schedule</span> - {{ title }}
        </h1>
      </div>
    </div>
  </template>
  
  <style scoped>
  .schedule-text {
    font-size: 32px; 
    display: block; 
    text-align: left; 
  }
  </style>
  
  <script setup lang="ts">
  import { useShiftsStore } from '@/stores/shiftsStore';
  import { onMounted, ref, watch } from 'vue';

  
defineProps<{ title: String }>();

const shiftsStore = useShiftsStore();
const shiftTitleFilter = ref<string>('');

onMounted(() => {
  shiftsStore.load();
});

watch(shiftTitleFilter, (title) => {
  shiftsStore.filterShiftsByTitle(title);
});


</script>