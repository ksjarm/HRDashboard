<template>
  <div
    class="min-h-screen bg-white-50 py-12 px-4 sm:px-6 lg:px=8 text-dark-300"
  >
    <div class="flex">
      <button
        @click.prevent="submitForm"
        class="group relative w-50 h-15 flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-200 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
      >
        <h1 class="newEmployeeButton">Add new employee</h1>
      </button>
      <input
        type="text"
        v-model="employeesNameFilter"
        placeholder="Employees name"
        class="border-2 ml-auto"
      />
    </div>
    <div class="text-center">
      <div class="bg-indigo-200 flex-1">
        <h1 class="font-bold mb-10 mt-10 heading">{{ title }}</h1>
      </div>
      <DataTable :value="employees">
        <Column field="name" header="Name" />
        <Column field="surname" header="Surname" />
        <Column field="position" header="Position" />
        <Column field="status" header="Status"></Column>
        <Column>
          <template #body="{ data }">
            <div>
              <router-link
                class="border bg-indigo-200 text-blue-900 py-0 px-2 mx-2 border-red-900 font-bold"
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
              <button
                class="border bg-green-400 text-green-900 py-0 px-3 ml-2 border-green-900 font-bold"
                @click="getFullInfo(data)"
              >
                ...
              </button>
            </div>
          </template>
        </Column>
      </DataTable>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Employee } from '@/modules/employee';
import { useEmployeesStore } from '@/stores/employeesStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';

const auth = useAuthStore();
const { isAuthenticated } = storeToRefs(auth);

const employeesStore = useEmployeesStore();
const { employees } = storeToRefs(employeesStore);
const employeesNameFilter = ref<string>('');
defineProps<{ title: String }>();

onMounted(() => {
  if (!isAuthenticated.value) {
    router.push({ name: 'Log in' });
  }
  employeesStore.load();
});

watch(employeesNameFilter, (title) => {
  employeesStore.filterEmployeesByName(title);
});

const remove = (employee: Employee) => {
  employeesStore.deleteEmployee(employee);
};
const getFullInfo = (employee: Employee) => {
  router.push({
    name: 'Get full info',
    query: { id: String(employee.id) },
  });
};

const router = useRouter();

const submitForm = () => {
  router.push({ name: 'Add employee' });
};
</script>

<style>
.heading {
  font-size: x-large;
}
.newEmployeeButton {
  font-size: large;
  margin-top: 10px;
  color: black;
}
</style>
