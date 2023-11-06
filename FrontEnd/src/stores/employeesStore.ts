import { Employee } from '@/modules/employee';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';

export const useEmployeesStore = defineStore('employeesStore', () => {
  const apiGetEmployees = useApi<Employee[]>('employees');
  const employees = ref<Employee[]>([]);
  let allEmployees: Employee[] = [];

  const loadEmployees = async () => {
    await apiGetEmployees.request();
    if (apiGetEmployees.response.value) {
      return apiGetEmployees.response.value!;
    }
    return [];
  };
  const load = async () => {
    allEmployees = await loadEmployees();
    employees.value = allEmployees;
  };

  const getEmployeeById = (id: number) => {
    return allEmployees.find((employee) => employee.id === id);
  };

  const addEmployee = async (employee: Employee) => {
    const apiAddEmployee = useApi<Employee>('employees', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employee),
    });

    await apiAddEmployee.request();
    if (apiAddEmployee.response.value) {
      allEmployees.push(apiAddEmployee.response.value!);
      employees.value = allEmployees;
    }
  };

  const updateEmployee = async (employee: Employee) => {
    const apiAddEmployee = useApi<Employee>('employees/' + employee.id, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employee),
    });

    await apiAddEmployee.request();
    if (apiAddEmployee.response.value) {
      load();
    }
  };

  const deleteEmployee = async (employee: Employee) => {
    const deleteEmployeeRequest = useApiRawRequest(`employees/${employee.id}`, {
      method: 'DELETE',
    });

    const res = await deleteEmployeeRequest();

    if (res.status === 204) {
      let id = allEmployees.indexOf(employee);

      if (id !== -1) {
        allEmployees.splice(id, 1);
      }

      id = employees.value.indexOf(employee);

      if (id !== -1) {
        employees.value.splice(id, 1);
      }
    }
  };

  const filterEmployeesByName = (employeesNameFilter: string) => {
    employees.value = allEmployees.filter((x) =>
      x.name.startsWith(employeesNameFilter),
    );
  };

  const apiGetCurrentEmployeeCount = useApi<number>(
    'employees/currentEmployeeCount',
  );

  const getCurrentEmployeeCount = async () => {
    await apiGetCurrentEmployeeCount.request();
    if (apiGetCurrentEmployeeCount.response.value) {
      return apiGetCurrentEmployeeCount.response.value!;
    }
    return 0;
  };

  return {
    employees,
    load,
    getEmployeeById,
    addEmployee,
    updateEmployee,
    deleteEmployee,
    filterEmployeesByName,
    getCurrentEmployeeCount,
  };
});
