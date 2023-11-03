import { Notifications } from '@/modules/notifications';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';

export const useNotificationsStore = defineStore('notificationsStore', () => {
  const apiGetNotifications = useApi<Notifications[]>('notifications');
  const notifications = ref<Notifications[]>([]);
  let allNotifications: Notifications[] = [];

  const loadNotifications = async () => {
    await apiGetNotifications.request();

    if (apiGetNotifications.response.value) {
      return apiGetNotifications.response.value!;
    }

    return [];
  };

  const load = async () => {
    allNotifications = await loadNotifications();
    notifications.value = allNotifications;
  };

  const getNotificationById = (id: number) => {
    return allNotifications.find((notification) => notification.NotificationId === id);
  };
//
  // const addNotifications = async (notification: Notifications) => {
  //   const apiNotification = useApi<Notifications>('notifications', {
  //     method: 'POST',
  //     headers: {
  //       Accept: 'application/json',
  //       'Content-Type': 'application/json',
  //     },
  //     body: JSON.stringify(notification),
  //   });

  //   await apiNotification.request();
  //   if (apiNotification.response.value) {
  //     allNotifications.push(apiNotification.response.value!);
  //     notifications.value = allNotifications;
  //   }
  // };

  // const updateNotifications = async (notification: Notifications) => {
  //   const apiAddExercise = useApi<Notifications>('notifications/' + notification.NotificationId, {
  //     method: 'PUT',
  //     headers: {
  //       Accept: 'application/json',
  //       'Content-Type': 'application/json',
  //     },
  //     body: JSON.stringify(notification),
  //   });

  //   await apiAddExercise.request();
  //   if (apiAddExercise.response.value) {
  //     load();
  //   }
  // };
//
  const deleteNotification = async (notification: Notifications) => {
    const deleteExerciseRequest = useApiRawRequest(`exercises/${notification.NotificationId}`, {
      method: 'DELETE',
    });

    const res = await deleteExerciseRequest();

    if (res.status === 204) {
      let id = allNotifications.indexOf(notification);

      if (id !== -1) {
        allNotifications.splice(id, 1);
      }

      id = notifications.value.indexOf(notification);

      if (id !== -1) {
        notifications.value.splice(id, 1);
      }
    }
  };

  const filterNotificationsByMessage = (employeesNameFilter: string) => {
    notifications.value = allNotifications.filter((x) =>
      x.Message.startsWith(employeesNameFilter),
    );
  };


  return {
    notifications,
    load,
    getNotificationById,
    // addNotifications,
    // updateNotifications,
    deleteNotification,
    filterNotificationsByMessage,
  };
});



