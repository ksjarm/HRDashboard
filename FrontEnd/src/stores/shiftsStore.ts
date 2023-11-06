import { Shift } from '@/modules/shift';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';


export const useShiftsStore = defineStore('shiftsStore', () => {
    const apiGetShifts = useApi<Shift[]>('shifts');
  const shifts = ref<Shift[]>([]);
  let allShifts: Shift[] = [];

  
  const loadShifts = async () => {
    await apiGetShifts.request();

    if (apiGetShifts.response.value) {
      return apiGetShifts.response.value!;
    }

    return [];
  };
  const load = async () => {
    allShifts = await loadShifts();
    shifts.value = allShifts;
  };

  const getShiftById = (id: number) => {
    return allShifts.find((shift) => shift.id === id);
  };

  const addShift = async (shift: Shift) => {
    const apiAddShift = useApi<Shift>('shifts', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(shift),
    });

    await apiAddShift.request();
    if (apiAddShift.response.value) {
      allShifts.push(apiAddShift.response.value!);
      shifts.value = allShifts;
    }
  };

  const updateShift = async (shift: Shift) => {
    try {
      const apiUpdateShift = useApi<Shift>('shifts/' + shift.id, {
        method: 'PUT',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(shift),
      });
  
      await apiUpdateShift.request();
  
      if (apiUpdateShift.response.value) {
        load(); // Refresh the shifts after a successful update
      }
    } catch (error) {
      console.error('Error updating shift:', error);
      // Handle the error (e.g., display an error message)
    }
  };
  
  const deleteShift = async (shift: Shift) => {
    const deleteShiftRequest = useApiRawRequest(`shifts/${shift.id}`, {
      method: 'DELETE',
    });

    const res = await deleteShiftRequest();

    if (res.status === 204) {
      let id = allShifts.indexOf(shift);

      if (id !== -1) {
        allShifts.splice(id, 1);
      }

      id = shifts.value.indexOf(shift);

      if (id !== -1) {
        shifts.value.splice(id, 1);
      }
    }
  };

  const filterShiftsByTitle = (shiftTitleFilter: string) => {
    shifts.value = allShifts.filter((x) =>
      x.title.startsWith(shiftTitleFilter),
    );
  };

  return {
    shifts,
    load,
    getShiftById,
    addShift,
    updateShift,
    deleteShift,
    filterShiftsByTitle,
  };
});
