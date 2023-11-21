import { Shift } from '@/modules/shift';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';
import { useNotificationsStore } from '@/stores/notificationsStore';


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

      const notificationsStore = useNotificationsStore();
      await notificationsStore.addNotifications({
        message: `Shift added: ${shift.title}`,
        date: new Date(), 
        type: 'Shift Added',
      });
    }
  };

  const updateShift = async (shift: Shift) => {
  
      const apiAddShift = useApi<Shift>('shifts/' + shift.id, {
        method: 'PUT',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(shift),
      });
  
      await apiAddShift.request();
  
      if (apiAddShift.response.value) {
        load(); // Refresh the shifts after a successful update
       
        const notificationsStore = useNotificationsStore();
        await notificationsStore.addNotifications({
          message: `Shift updated: ${shift.title}`,
          date: new Date(),
          type: 'Shift Updated',
        });
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
      
      const notificationsStore = useNotificationsStore();
      await notificationsStore.addNotifications({
        message: `Shift deleted: ${shift.title}`,
        date: new Date(),
        type: 'Shift Deleted',
      });
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
