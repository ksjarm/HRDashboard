<template>
  <EmployeeList :title="title" />
</template>

<script setup lang="ts">
import EmployeeList from '@/components/EmployeeList.vue';

defineProps<{ title: String }>();
</script>
