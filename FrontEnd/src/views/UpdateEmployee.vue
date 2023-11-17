<template>
  <AddEmployeeForm :employee="employee" />
</template>

<script setup lang="ts">
import AddEmployeeForm from '@/components/AddEmployeeForm.vue';
import { Employee } from '@/modules/employee';
import { useEmployeesStore } from '@/stores/employeesStore';
import { onMounted, Ref, ref } from 'vue';
import { useRoute } from 'vue-router';

const { load, getEmployeeById } = useEmployeesStore();
const route = useRoute();

const employee: Ref<Employee> = ref<Employee>({
  id: 0,
  name: '',
  surname: '',
  gender: 'Male',
  dateOfBirth: '',
  email: '',
  phoneNumber: '',
  adress: '',
  position: '',
  salary: 0,
  status: '',
  photo: '',
});

onMounted(async () => {
  await load();

  const loadedEmployee = getEmployeeById(Number(route.params.id));
  if (loadedEmployee !== undefined) {
    employee.value = loadedEmployee;
  }
});
</script>
