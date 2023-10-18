import { Exercise } from '@/modules/exercise';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';

export const useExercisesStore = defineStore('exercisesStore', () => {
  const apiGetExercises = useApi<Exercise[]>('exercises');
  const exercises = ref<Exercise[]>([]);
  let allExercises: Exercise[] = [];

  const loadExercises = async () => {
    await apiGetExercises.request();

    if (apiGetExercises.response.value) {
      return apiGetExercises.response.value!;
    }

    return [];
  };

  const load = async () => {
    allExercises = await loadExercises();
    exercises.value = allExercises;
  };

  const getExerciseById = (id: number) => {
    return allExercises.find((exercise) => exercise.id === id);
  };

  const addExercise = async (exercise: Exercise) => {
    const apiAddExercise = useApi<Exercise>('exercises', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(exercise),
    });

    await apiAddExercise.request();
    if (apiAddExercise.response.value) {
      allExercises.push(apiAddExercise.response.value!);
      exercises.value = allExercises;
    }
  };

  const updateExercise = async (exercise: Exercise) => {
    const apiAddExercise = useApi<Exercise>('exercises/' + exercise.id, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(exercise),
    });

    await apiAddExercise.request();
    if (apiAddExercise.response.value) {
      load();
    }
  };

  const deleteExercise = async (exercise: Exercise) => {
    const deleteExerciseRequest = useApiRawRequest(`exercises/${exercise.id}`, {
      method: 'DELETE',
    });

    const res = await deleteExerciseRequest();

    if (res.status === 204) {
      let id = allExercises.indexOf(exercise);

      if (id !== -1) {
        allExercises.splice(id, 1);
      }

      id = exercises.value.indexOf(exercise);

      if (id !== -1) {
        exercises.value.splice(id, 1);
      }
    }
  };

  const filterExercisesByTitle = (exerciseTitleFilter: string) => {
    exercises.value = allExercises.filter((x) =>
      x.title.startsWith(exerciseTitleFilter),
    );
  };

  return {
    exercises,
    load,
    getExerciseById,
    addExercise,
    updateExercise,
    deleteExercise,
    filterExercisesByTitle,
  };
});
