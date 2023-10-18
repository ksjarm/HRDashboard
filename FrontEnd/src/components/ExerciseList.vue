<template>
  <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px=8 text-dark-300">
    <div class="text-center">
      <h1 class="font-bold">{{ title }}</h1>
      <DataTable :value="exercises">
        <template #header>
          <div class="flex">
            <h1 class="font-bold">{{ title }}</h1>
            <input
              type="text"
              v-model="exerciseTitleFilter"
              placeholder="Harjutuse nime filter"
              class="border-2 ml-auto"
            />
          </div>
        </template>
        <Column field="title" header="Nimi" />
        <Column field="description" header="Kirjeldus" />
        <Column>
          <template #body="{ data }">
            <div>
              <router-link
                class="border bg-blue-400 text-blue-900 py-0 px-2 mx-2 border-red-900 font-bold"
                :to="'update/' + data.id"
              >
                ⭮
              </router-link>

              <button
                class="border bg-red-400 text-red-900 py-0 px-3 border-red-900 font-bold"
                @click="remove(data)"
              >
                X
              </button>
            </div>
          </template>
        </Column>
      </DataTable>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Exercise } from '@/modules/exercise';
import { useExercisesStore } from '@/stores/exercisesStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';

defineProps<{ title: String }>();

const exercisesStore = useExercisesStore();
const { exercises } = storeToRefs(exercisesStore);
const exerciseTitleFilter = ref<string>('');

onMounted(() => {
  exercisesStore.load();
});

watch(exerciseTitleFilter, (title) => {
  exercisesStore.filterExercisesByTitle(title);
});

const remove = (exercise: Exercise) => {
  exercisesStore.deleteExercise(exercise);
};
</script>
