<template>
  <div class="container">
    <h1 class="heading">Notifications History</h1>
    <div class="header">
      
      <div class="filters">
        <div class="date-filters">
          <label for="start-date">Start Date:</label>
          <input type="date" id="start-date" v-model="startDate" placeholder="Start date">

          <label for="end-date">End Date:</label>
          <input type="date" id="end-date" v-model="endDate" placeholder="End date">
        </div>
        <input type="text" v-model="messageFilter" placeholder="Filter">
      </div>
      <div class="button-container">
        <button @click="showConfirmation = true" class="delete-all-btn">Clear All Notifications</button>
      </div>
    </div>

    <div v-if="showConfirmation" class="confirmation-overlay">
      <div class="confirmation-dialog">
        <h2>Confirm Deletion</h2>
        <p>Are you sure you want to clear all notifications?</p>
        <button @click="deleteAllNotifications">Yes</button>
        <button @click="showConfirmation = false">No</button>
      </div>
    </div>
    
    <div class="notifications-container">
      <div v-if="filteredNotifications.length > 0">
        <div
          v-for="notification in filteredNotifications"
          :key="notification.notificationId"
          class="notification-block"
        >
        <span v-if="notification.isNew" class="hot-dot"></span>

          <div class="date-time">
            <strong>{{ formatDate(notification.date) }}</strong>
            <p>{{ formatTime(notification.date) }}</p>
          </div>
          <div class="type-message">
            <strong>{{ notification.type }}:</strong>
            <p>{{ notification.message }}</p>
          </div>
          <button
            class="delete-button"
            @click="deleteNotification(notification)"
          >
            X
          </button>
        </div>
      </div>
      <div v-else>
        <p>No notifications found.</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref , computed} from 'vue';
import { useNotificationsStore } from '@/stores/notificationsStore';
import { Notification } from '@/modules/notifications';
import { useAuthStore } from '@/stores/authStore';
import { useRouter } from 'vue-router';
import { storeToRefs } from 'pinia';


const router = useRouter();
const auth = useAuthStore();
const { isAuthenticated } = storeToRefs(auth);
const notificationsStore = useNotificationsStore();
const notifications = ref(notificationsStore.notifications);

onMounted(async () => {
  if (!isAuthenticated.value) {
    router.push({ name: 'Log in' });
  }
  await notificationsStore.load();
  notifications.value.sort(
    (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime(),
  );
});

const deleteNotification = async (notification: Notification) => {
  await notificationsStore.deleteNotification(notification);
  notifications.value = notifications.value.filter(
    (n) => n.notificationId !== notification.notificationId,
  );
};

const showConfirmation = ref(false);

const deleteAllNotifications = async () => {
  
  await notificationsStore.deleteAllNotifications();
notifications.value = [];
showConfirmation.value = false;
};

const formatDate = (date: Date) => {
  return new Date(date).toLocaleDateString();
};

const formatTime = (date: Date) => {
  return new Date(date).toLocaleTimeString();
};

const startDate = ref('');
const endDate = ref('');
const messageFilter = ref('');
const TWO_HOURS_IN_MS = 2 * 60 * 60 * 1000;;

const filteredNotifications = computed(() => {
  const now = new Date().getTime();

  return notifications.value
  .filter(notification => {
      const notificationDate = new Date(notification.date);

      let start = startDate.value ? new Date(startDate.value) : null;
      let end = endDate.value ? new Date(endDate.value) : null;
      if (end) {
        end.setHours(23, 59, 59, 999);
      }
      const messageLower = notification.message.toLowerCase();
      const messageFilterLower = messageFilter.value.toLowerCase();

      return (!start || notificationDate >= start) 
        && (!end || notificationDate <= end)
        && (!messageFilter.value || messageLower.includes(messageFilterLower));
    })
  .map(notification => {
      const notificationDate = new Date(notification.date).getTime();
      const isNew = now - notificationDate < TWO_HOURS_IN_MS;
      return { ...notification, isNew };
    });

});

</script>

<style scoped>
.hot-dot {
  height: 10px;
  width: 10px;
  background-color: rgb(141, 108, 218); 
  border-radius: 50%;
  display: inline-block;
  margin-left: 10px;
}
.filters {
  display: flex;
  align-items: center;
  gap: 15px; 
}
.date-filters {
  display: flex;
  flex-direction: column; 
  gap: 5px; 
}
.date-filters label {
  font-size: 0.9em;
  color: #333;
}
.filters input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  flex-grow: 1; 
}
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 20px;
}

.heading {
  margin: 0;
  font-weight: bold;  
}  

.confirmation-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(0, 0, 0, 0.5);  
  z-index: 1000;
}
.confirmation-dialog {
  background-color: white;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
  z-index: 1001; 
  text-align: center;
}
.confirmation-dialog h2 {
  margin-bottom: 15px;
}
.confirmation-dialog button {
  margin: 10px;
  padding: 10px 15px;
  border: 1px solid transparent;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s ease;
}
.confirmation-dialog button:hover {
  background-color: #333; 
  color: #fff; 
  border-color: #444; 
}
.button-container {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 20px; 
  padding-left: 15px;
  
}
.notifications-container {
  display: flex;
  flex-direction: column;
}

.notification-block {
  display: flex;
  align-items: center;
  position: relative;
  border: 1px solid #ccc;
  border-radius: 8px;
  background-color: #f9f9f9;
  margin-bottom: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
.delete-all-btn {
  background-color: #ff4d4d; 
  color: white; 
  padding: 10px 20px; 
  border: none; 
  border-radius: 5px; 
  cursor: pointer; 
  font-weight: bold;
  margin-top: 20px; 
  transition: background-color 0.3s; 
}
.delete-all-btn:hover {
  background-color: #cc0000; 
}
.date-time {
  padding: 16px;
  border-right: 1px solid #ccc;
  flex-basis: 20%;
  text-align: center;
}
.type-message {
  padding: 16px;
  flex-grow: 1;
}
.date-time strong {
  margin-bottom: 8px;
}
.delete-button {
  position: absolute;
  top: 10px;
  right: 10px;
  background-color: red;
  color: white;
  border: none;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-weight: bold;
  cursor: pointer;
}
.delete-button:hover {
  background-color: darkred;
}
</style>
