import { Employee } from '@/modules/employee';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';
import { useNotificationsStore } from '@/stores/notificationsStore';
import { useAuthStore } from '@/stores/authStore';

export const useEmployeesStore = defineStore('employeesStore', () => {
  const authStore = useAuthStore();
  const employees = ref<Employee[]>([]);
  let allEmployees: Employee[] = [];

  const loadEmployees = async () => {
    const apiGetEmployees = useApi<Employee[]>('employees', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });
    await apiGetEmployees.request();
    if (apiGetEmployees.response.value) {
      return apiGetEmployees.response.value!;
    }
    return [];
  };
  const load = async () => {
    allEmployees = await loadEmployees();
    console.log('im here');
    employees.value = allEmployees;
  };

  const getEmployeeById = (id: number) => {
    return allEmployees.find((employee) => employee.id === id);
  };

  const addEmployee = async (employee: Employee) => {
    const apiAddEmployee = useApi<Employee>('employees', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employee),
    });

    await apiAddEmployee.request();
    if (apiAddEmployee.response.value) {
      allEmployees.push(apiAddEmployee.response.value!);
      employees.value = allEmployees;

      const notificationsStore = useNotificationsStore();
      const newNotification = {
        message: `New employee added: ${employee.name} ${employee.surname}`,
        date: new Date(),
        type: 'Employee Added',
      };
      await notificationsStore.addNotifications(newNotification);
    }
  };

  const updateEmployee = async (employee: Employee) => {
    const apiAddEmployee = useApi<Employee>('employees/' + employee.id, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(employee),
    });

    await apiAddEmployee.request();
    if (apiAddEmployee.response.value) {
      load();

      const notificationsStore = useNotificationsStore();
      const updateNotification = {
        message: `Employee updated: ${employee.name} ${employee.surname}`,
        date: new Date(),
        type: 'Employee Updated',
      };
      await notificationsStore.addNotifications(updateNotification);
    }
  };

  const deleteEmployee = async (employee: Employee) => {
    const deleteEmployeeRequest = useApiRawRequest(`employees/${employee.id}`, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + authStore.token },
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

      const notificationsStore = useNotificationsStore();
      const deleteNotification = {
        message: `Employee deleted: ${employee.name} ${employee.surname}`,
        date: new Date(),
        type: 'Employee Deleted',
      };
      await notificationsStore.addNotifications(deleteNotification);
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

  const getEmployeeShiftsById = async (employeeId: number) => {
    const getEmployeeShifts = useApiRawRequest(
      `employees/${employeeId}/shifts`,
      {
        method: 'GET',
      },
    );
    const res = await getEmployeeShifts();

    if (res.status === 200) {
      const data = await res.json();
      return data;
    }

    return [];
  };

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
    getEmployeeShiftsById,
  };
});
