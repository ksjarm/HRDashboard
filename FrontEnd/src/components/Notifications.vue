
<template>
  <div>
    <div class="text-center">
      <div class="bg-indigo-200 flex-1">
        <h1 class="font-bold mb-10 mt-10 heading"> Notifications History</h1>
      </div>
    </div>
    <div class="notifications-container">
      <div v-if="notifications.length > 0">
        <div v-for="notification in notifications" 
          :key="notification.notificationId" 
            class="notification-block">
        <h3>{{ formatDate(notification.date) }}</h3>
        <p><strong>Type:</strong> {{ notification.type }}</p>
        <p><strong>Message:</strong> {{ notification.message }}</p>
        
        </div>
      </div>
      <div v-else>
        <p>No notifications found.</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useNotificationsStore } from '@/stores/notificationsStore';

const { notifications, load } = useNotificationsStore();

onMounted(async () => {
  await load(); 
});

const formatDate = (date: Date) => {
  return new Date(date).toLocaleDateString();
};


</script>

<style scoped>
.notifications-container {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.notification-block {
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 16px;
  background-color: #f9f9f9;
}
</style>