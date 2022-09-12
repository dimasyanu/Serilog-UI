<script setup>
import axios from 'axios';
import Filters from './components/Filters.vue';
import LogList from './components/LogList.vue';
import Pagination from './components/Pagination.vue';
import 'bootstrap/js/src/collapse';
import 'bootstrap/dist/css/bootstrap.min.css';
</script>

<template>
  <div class="p-5" style="height: 100vh">
    <div v-if="loading" class="row justify-content-center align-items-center" style="height: 100%">
      <div class="spinner-border text-secondary m-3" style="width: 3rem; height: 3rem" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
    <div v-show="!loading">
      <div class="row">
        <div class="col-lg-6 col-sm-12">
          <Filters :loadingLogs="loadingLogs" @apply="applyFilter" />
        </div>
        <div class="col-lg-6 col-sm-12">
          <div class="d-flex justify-content-end m-1" v-if="!loadingLogs">
            <Pagination
              :totalItems="totalItems"
              :pageSize="parseInt(filters.pageSize)"
              :startIndex="startIndex"
              @goToPage="goToPage"
            />
          </div>
        </div>
      </div>

      <div class="row">
        <div v-if="loadingLogs" class="text-center">
          <div class="spinner-border text-secondary m-3" style="width: 3rem; height: 3rem" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
        <LogList :items="items" v-else />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  // inject: ['mq'],
  components: [Filters, LogList, Pagination],
  data() {
    return {
      items: [],
      loading: false,
      loadingLogs: false,
      startIndex: 0,
      totalItems: 0,
      queryString: null,
      filters: { pageSize: 25 },
    };
  },
  mounted() {
    this.loading = true;
  },
  methods: {
    applyFilter(filters) {
      this.filters = filters;
      this.startIndex = 0;
      this.totalItems = 0;
      this.getLogs();
    },
    getLogs() {
      this.items = [];
      this.loading = false;
      this.loadingLogs = true;

      var query =
        'filename=' + this.filters.date + '&pageSize=' + this.filters.pageSize + '&level=' + this.filters.level;

      axios
        .get(import.meta.env.VITE_ENDPOINT_GET_ITEMS + '?' + query + '&startIndex=' + this.startIndex)
        .then((res) => {
          this.loading = false;
          this.loadingLogs = false;

          this.items = res.data.data.items;
          this.totalItems = res.data.data.totalItems;
        });
    },
    goToPage(startIndex) {
      this.startIndex = startIndex;
      this.getLogs();
    },
  },
};
</script>
