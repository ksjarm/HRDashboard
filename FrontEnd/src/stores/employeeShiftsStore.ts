import { EmployeeShift } from '@/modules/employeeShift';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';
import { useAuthStore } from '@/stores/authStore';

export const useEmployeeShiftsStore = defineStore('employeeShiftsStore', () => {
  const authStore = useAuthStore();
  const employeeShifts = ref<EmployeeShift[]>([]);
  let allEmployeeShifts: EmployeeShift[] = [];

  const loadEmployeeShifts = async () => {
    const apiGetEmployeeShifts = useApi<EmployeeShift[]>('employeeShifts', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });
    console.log(apiGetEmployeeShifts.response.value);
    await apiGetEmployeeShifts.request();
    console.log('Response from loadEmployeeShifts:', apiGetEmployeeShifts.response.value);
    if (apiGetEmployeeShifts.response.value) {
      return apiGetEmployeeShifts.response.value!;
    }
    return [];
  };
  
  const load = async () => {
    allEmployeeShifts = await loadEmployeeShifts();
    employeeShifts.value = allEmployeeShifts;
  };

  const addEmployeeShift = async (employeeShift: EmployeeShift) => {
    const apiAddEmployeeShift = useApi<EmployeeShift>('employeeShifts', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employeeShift),
    });

    await apiAddEmployeeShift.request();
    if (apiAddEmployeeShift.response.value) {
      const addedEmployeeShift = apiAddEmployeeShift.response.value;
      employeeShifts.value.push(addedEmployeeShift);
    }
  };

  const updateEmployeeShift = async (employeeShift: EmployeeShift) => {
    const apiUpdateEmployeeShift = useApi<EmployeeShift>(
      `employeeShifts/${employeeShift.employeeId}`,
      {
        method: 'PUT',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(employeeShift),
      },
    );

    await apiUpdateEmployeeShift.request();
    if (apiUpdateEmployeeShift.response.value) {
      const updatedEmployeeShift = apiUpdateEmployeeShift.response.value;
      const index = employeeShifts.value.findIndex(
        (es) => es.employeeId === updatedEmployeeShift.employeeId,
      );
      if (index !== -1) {
        employeeShifts.value[index] = updatedEmployeeShift;
      }
    }
  };

  const deleteEmployeeShift = async (employeeShift: EmployeeShift) => {
    const deleteEmployeeShiftRequest = useApiRawRequest(
      `employeeShifts/${employeeShift.employeeId}`,
      {
        method: 'DELETE',
      },
    );

    const res = await deleteEmployeeShiftRequest();

    if (res.status === 204) {
      const index = employeeShifts.value.findIndex(
        (es) => es.employeeId === employeeShift.employeeId,
      );
      if (index !== -1) {
        employeeShifts.value.splice(index, 1);
      }
    }
  };

  return {
    employeeShifts,
    load,
    addEmployeeShift,
    updateEmployeeShift,
    deleteEmployeeShift,
  };
});
