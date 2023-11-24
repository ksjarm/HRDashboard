<template>
    <HRFullInfo :name="name" />
  </template>
  
  <script setup lang="ts">
  import HRFullInfo from '@/components/HRFullInfoForm.vue';
  
  defineProps<{ name: String }>();
  </script>
  