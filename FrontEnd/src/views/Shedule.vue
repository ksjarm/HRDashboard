<template>
    <Shedule :title="title" />
  </template>
  
  <script setup lang="ts">
  import Shedule from '@/components/Shedule.vue';
  
  defineProps<{ title: String }>();
  </script>
  