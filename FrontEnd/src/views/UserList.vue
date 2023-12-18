<template>
    <UserList :name="name" />
  </template>
  
  <script setup lang="ts">
  import UserList from '@/components/UserList.vue';
  
  defineProps<{ name: String }>();
  </script>
  