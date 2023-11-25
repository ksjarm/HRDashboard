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
  gender: 'Male',
  dateOfBirth: '',
  email: '',
  phoneNumber: '',
  adress: '',
  position: 'Vendor',
  salary: 0,
  status: 'Active',
  photo: '',
});
</script>
