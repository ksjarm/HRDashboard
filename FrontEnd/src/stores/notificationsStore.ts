import { Notification } from '@/modules/notifications';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';

export const useNotificationsStore = defineStore('notificationsStore', () => {
  const apiGetNotifications = useApi<Notification[]>('notifications');
  const notifications = ref<Notification[]>([]);
  let allNotifications: Notification[] = [];

  const loadNotifications = async () => {
    await apiGetNotifications.request();

    if (apiGetNotifications.response.value) {
      return apiGetNotifications.response.value!;
    }

    return [];
  };

  const load = async () => {
    allNotifications = await loadNotifications();
    console.log(allNotifications);
    notifications.value = allNotifications;
  };

  const getNotificationById = (id: number) => {
    return allNotifications.find((notification) => notification.notificationId === id);
  };

  const addNotifications = async (notification: Notification) => {
    const apiNotification = useApi<Notification>('notifications', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(notification),
    });

    await apiNotification.request();
    if (apiNotification.response.value) {
      allNotifications.push(apiNotification.response.value!);
      notifications.value = allNotifications;
    }
  };

  const updateNotifications = async (notification: Notification) => {
    const apiNotification = useApi<Notification>('notifications/' + notification.notificationId, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(notification),
    });

    await apiNotification.request();
    if (apiNotification.response.value) {
      load();
    }
  };

  const deleteNotification = async (notification: Notification) => {
    const deleteNotificationRequest = useApiRawRequest(`notifications/${notification.notificationId}`, {
      method: 'DELETE',
    });

    const res = await deleteNotificationRequest();

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

  const deleteAllNotifications = async () => {
    const deleteAllRequest = useApiRawRequest('notifications/deleteAll', {
      method: 'DELETE',
    });
    await deleteAllRequest();
    notifications.value = [];
  };

  const filterNotificationsByMessage = (employeesNameFilter: string) => {
    notifications.value = allNotifications.filter((x) =>
      x.message.startsWith(employeesNameFilter),
    );
  };


  return {
    notifications,
    load,
    getNotificationById,
    addNotifications,
    updateNotifications,
    deleteNotification,
    deleteAllNotifications,
    filterNotificationsByMessage,
  };
});



