import { EmployeeShift } from '@/modules/employeeShift';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';

export const useEmployeeShiftsStore = defineStore('employeeShiftsStore', () => {
  const apiGetEmployeeShifts = useApi<EmployeeShift[]>('employeeShifts');
  const employeeShifts = ref<EmployeeShift[]>([]);

  const loadEmployeeShifts = async () => {
    await apiGetEmployeeShifts.request();
    if (apiGetEmployeeShifts.response.value) {
      employeeShifts.value = apiGetEmployeeShifts.response.value!;
    }
  };

  const addEmployeeShift = async (employeeShift: EmployeeShift) => {
    // Implement the logic to add an employee shift through your API here
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
      const addedEmployeeShift = apiAddEmployeeShift.response.value!;
      // Update the employee shifts state with the newly added employee shift
      employeeShifts.value.push(addedEmployeeShift);
    }
  };

  const updateEmployeeShift = async (employeeShift: EmployeeShift) => {
    // Implement the logic to update an employee shift through your API here
    const apiUpdateEmployeeShift = useApi<EmployeeShift>(`employeeShifts/${employeeShift.employeeId}`, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employeeShift),
    });

    await apiUpdateEmployeeShift.request();
    if (apiUpdateEmployeeShift.response.value) {
      const updatedEmployeeShift = apiUpdateEmployeeShift.response.value!;
      // Update the employee shifts state with the updated employee shift
      const index = employeeShifts.value.findIndex((es) => es.employeeId === updatedEmployeeShift.employeeId);
      if (index !== -1) {
        employeeShifts.value[index] = updatedEmployeeShift;
      }
    }
  };

  const deleteEmployeeShift = async (employeeShift: EmployeeShift) => {
    // Implement the logic to delete an employee shift through your API here
    const deleteEmployeeShiftRequest = useApiRawRequest(`employeeShifts/${employeeShift.employeeId}`, {
      method: 'DELETE',
    });

    const res = await deleteEmployeeShiftRequest();

    if (res.status === 204) {
      // Remove the deleted employee shift from the employee shifts state
      const index = employeeShifts.value.findIndex((es) => es.employeeId === employeeShift.employeeId);
      if (index !== -1) {
        employeeShifts.value.splice(index, 1);
      }
    }
  };

  return {
    employeeShifts,
    loadEmployeeShifts,
    addEmployeeShift,
    updateEmployeeShift,
    deleteEmployeeShift,
  };
});
