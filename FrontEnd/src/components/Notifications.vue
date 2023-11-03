<template>
  <div>
    <div class="text-center">
      <div class="bg-indigo-200 flex-1">
        <h1 class="font-bold mb-10 mt-10 heading"> Notifications History</h1>
      </div>
    </div>
    <div class="search-bar">
      <input
        type="text"
        v-model="notificationMessageFilter"
        placeholder="Search by keyword or date"
      />
    </div>
    <div class="notification-list">
     


    </div>

  </div>
  </template>
  
  <script setup lang="ts">
  import { useNotificationsStore } from '@/stores/notificationsStore';
  import { onMounted , ref, watch} from 'vue';
  
  defineProps<{ title: String }>();
  
  const notificationsStore = useNotificationsStore();
  const notificationMessageFilter = ref<string>('');

  onMounted(() => {
    notificationsStore.load();
  });

  watch(notificationMessageFilter, (title) => {
    notificationsStore.filterNotificationsByMessage(title);
  });
  </script>




<!--   
  <template>
    <div>
      <div class="text-center">
      <div class="bg-indigo-200 flex-1">
        <h1 class="font-bold mb-10 mt-10 heading"> Notifications History</h1>
      </div>
    </div>
      <!-- Search Bar -->
      <div class="my-4">
        <input
          v-model="searchTerm"
          type="text"
          placeholder="Search by date or keyword"
          class="border p-2 rounded-lg w-full"
        />
      </div>
  
      <!-- List of Notifications -->
      <div v-if="filteredNotifications.length > 0">
        <div
          v-for="notification in filteredNotifications"
          :key="notification.NotificationId"
        >
          <div class="notification-card">
            <div class="notification-info">
              <div class="notification-text">{{ notification.Message }}</div>
              <div class="notification-date">
                {{ formatDate(notification.Date) }}
              </div>
            </div>
            <button
              @click="deleteNotification(notification)"
              class="delete-button"
            >
              Delete
            </button>
          </div>
        </div>
      </div>
      <div v-else>
        <p>No notifications found.</p>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useNotificationsStore } from '@/stores/notificationsStore';
  import { Notifications } from '@/modules/notifications';
  
  const notificationsStore = useNotificationsStore();
  
  // Search term for filtering
  const searchTerm = ref('');
  
  // Function to format date and time
  const formatDate = (date: Date) => {
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    };
    return date.toLocaleString(undefined, options);
  };
  
  // Filter notifications based on search term
  const filteredNotifications = computed(() => {
    if (searchTerm.value === '') {
      return notificationsStore.notifications;
    }
    const searchTermLowerCase = searchTerm.value.toLowerCase();
    return notificationsStore.notifications.filter((notification) => {
      return (
        formatDate(notification.Date)
          .toLowerCase()
          .includes(searchTermLowerCase) ||
        notification.Message.toLowerCase().includes(searchTermLowerCase)
      );
    });
  });
  
  // Function to delete a notification
  const deleteNotification = (notification: Notifications) => {
    notificationsStore.deleteNotification(notification);
  };
  </script>
  
  <style scoped>
  /* Add your CSS styles here for notification cards, search bar, etc. */
  .notification-card {
    background-color: #f7f7f7;
    border: 1px solid #e0e0e0;
    margin: 10px;
    padding: 10px;
    display: flex;
    justify-content: space-between;
  }
  
  .notification-info {
    flex: 1;
  }
  
  .notification-text {
    font-size: 16px;
    margin-bottom: 8px;
  }
  
  .notification-date {
    font-size: 12px;
  }
  
  .delete-button {
    background-color: #ff6961;
    color: white;
    border: none;
    padding: 5px 10px;
    border-radius: 5px;
    cursor: pointer;
  }
  </style>
   -->