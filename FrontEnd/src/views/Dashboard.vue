<template>
  <DashboardBlocks :title="title" />
</template>

<script setup lang="ts">
import DashboardBlocks from '@/components/DashboardBlocks.vue';
defineProps<{ title: String }>();
</script>
