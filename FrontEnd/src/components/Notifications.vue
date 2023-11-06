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




