<template>
  <AddEmployeeForm :employee="employee" />
</template>

<script setup lang="ts">
import AddEmployeeForm from '@/components/AddEmployeeForm.vue';
import { Employee } from '@/modules/employee';
import { Ref, ref } from 'vue';

const employee: Ref<Employee> = ref({
  id: 0,
  name: '',
  surname: '',
  gender: '',
  dateOfBirth: '',
  email: '',
  phoneNumber: '',
  adress: '',
  position: '',
  salary: 0,
  status: '',
});
</script>
