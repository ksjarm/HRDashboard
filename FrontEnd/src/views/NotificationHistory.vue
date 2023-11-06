<template>
    <Notifications :title="title" />
  </template>
  
  <script setup lang="ts">
  import Notifications from '@/components/Notifications.vue';
  
  defineProps<{ title: String }>();
  </script>
  