import { RouteRecordRaw, createRouter, createWebHistory } from 'vue-router';
import ExercisesVue from '@/views/Exercises.vue';
import AddExerciseVue from '@/views/AddExercise.vue';
import UpdateExerciseVue from '@/views/UpdateExercise.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Harjutused',
    component: ExercisesVue,
    props: { title: 'Harjutused' },
  },
  {
    path: '/newexercise',
    name: 'Lisa harjutus',
    component: AddExerciseVue,
  },
  {
    path: '/update/:id',
    name: 'Uuenda harjutust',
    component: UpdateExerciseVue,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
