<template>
  <AddExerciseForm :exercise="exercise" />
</template>

<script setup lang="ts">
import AddExerciseForm from '@/components/AddExerciseForm.vue';
import { Exercise } from '@/modules/exercise';
import { Ref, ref } from 'vue';

const exercise: Ref<Exercise> = ref({
  id: 0,
  title: '',
  description: '',
  intensity: 'normal',
  recommendedDurationInSeconds: 40,
  recommendedRestTimeInSecondsAfterExercise: 10,
  recommendedRestTimeInSecondsBeforeExercies: 10,
});
</script>
