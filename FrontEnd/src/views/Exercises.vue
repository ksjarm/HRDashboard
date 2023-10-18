<template>
  <ExerciseList :title="title" />
</template>

<script setup lang="ts">
import ExerciseList from '@/components/ExerciseList.vue';

defineProps<{ title: String }>();
</script>
