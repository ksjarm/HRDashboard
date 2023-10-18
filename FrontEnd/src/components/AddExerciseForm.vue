<template>
  <div
    class="min-h-full flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8"
  >
    <form class="max-w-md w-full space-y-8">
      <div class="mt-8 space-y-6">
        <div class="rounded-md shadow-sm -space-y-px">
          <div>
            <label for="name">Nimi</label>
            <input
              id="name"
              name="name"
              v-model="props.exercise.title"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Nimi"
            />
          </div>
          <div>
            <label for="description">Kirjeldus</label>
            <input
              id="description"
              name="description"
              v-model="props.exercise.description"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Kirjeldus"
            />
          </div>
        </div>

        <div>
          <button
            @click.prevent="submitForm"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          >
            <span class="absolute left-0 inset-y-0 flex items-center pl-3">
            </span>
            Lisa harjutus
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { Exercise } from '@/modules/exercise';
import { useExercisesStore } from '@/stores/exercisesStore';
import { useRouter } from 'vue-router';

const props = defineProps<{ exercise: Exercise }>();

const { addExercise, updateExercise } = useExercisesStore();
const router = useRouter();

const submitForm = () => {
  if (props.exercise.id === 0) {
    addExercise(props.exercise);
  } else {
    updateExercise(props.exercise);
  }

  props.exercise.title = '';
  props.exercise.description = '';

  router.push({ name: 'Harjutused' });
};
</script>
