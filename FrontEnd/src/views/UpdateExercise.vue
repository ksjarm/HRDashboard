<template>
  <AddExerciseForm :exercise="exercise" />
</template>

<script setup lang="ts">
import AddExerciseForm from '@/components/AddExerciseForm.vue';
import { Exercise } from '@/modules/exercise';
import { useExercisesStore } from '@/stores/exercisesStore';
import { onMounted, Ref, ref } from 'vue';
import { useRoute } from 'vue-router';

const { load, getExerciseById } = useExercisesStore();
const route = useRoute();

const exercise: Ref<Exercise> = ref<Exercise>({
  id: 0,
  title: '',
  description: '',
  intensity: 'normal',
  recommendedDurationInSeconds: 40,
  recommendedRestTimeInSecondsAfterExercise: 10,
  recommendedRestTimeInSecondsBeforeExercies: 10,
});

onMounted(async () => {
  await load();

  const loadedExercise = getExerciseById(Number(route.params.id));
  if (loadedExercise !== undefined) {
    exercise.value = loadedExercise;
  }
});
</script>
