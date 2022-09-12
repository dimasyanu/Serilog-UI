<script setup>
import axios from 'axios';
</script>
<template>
  <form class="row justify-content-center">
    <div class="col-auto">
      <my-select
        class="form-select form-select mb-3"
        name="filename"
        title="Filename"
        :options="filenames"
        v-model="model.date"
      ></my-select>
    </div>

    <div class="col-auto">
      <select class="form-select form-select mb-3" v-model="model.level" aria-label=".form-select">
        <option selected value="">All</option>
        <option value="error">Errors</option>
      </select>
    </div>

    <div class="col-auto">
      <select class="form-select form-select mb-3" v-model="model.pageSize" aria-label=".form-select">
        <option selected value="10">10</option>
        <option value="25">25</option>
        <option value="50">50</option>
        <option value="-1">All</option>
      </select>
    </div>

    <div class="col-auto">
      <div v-if="!loadingLogs" class="btn btn-primary btn" @click="refresh">Refresh</div>
      <div v-else class="btn btn-primary btn disabled" type="button" disabled>
        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
        Loading...
      </div>
    </div>
  </form>
</template>

<script>
export default {
  props: {
    loadingLogs: [Boolean],
  },
  data() {
    return {
      loading: false,
      filenames: [],
      model: {
        date: null,
        pageSize: 25,
        level: '',
      },
    };
  },
  mounted() {
    this.getFilenames();
  },
  methods: {
    refresh() {
      this.$emit('apply', this.model);
    },
    getFilenames() {
      this.loading = true;
      var self = this;
      axios
        .get(import.meta.env.VITE_ENDPOINT_GET_FILENAMES)
        .then((res) => {
          self.loading = false;
          self.filenames = res.data.data.items;
          if (self.filenames.length > 0) {
            self.model.date = self.filenames[0];
            return;
          }
        })
        .catch((err) => {
          console.log(err);
        });
    },
  },
  watch: {
    'model.date': function () {
      this.refresh();
    },
    'model.pageSize': function () {
      this.refresh();
    },
    'model.level': function () {
      this.refresh();
    },
  },
};
</script>
