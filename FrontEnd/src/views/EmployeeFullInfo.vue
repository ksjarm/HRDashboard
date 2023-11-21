<template>
  <EmployeeFullInfo :name="name" />
</template>

<script setup lang="ts">
import EmployeeFullInfo from '@/components/EmployeeFullInfoForm.vue';

defineProps<{ name: String }>();
</script>
