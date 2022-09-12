<script setup>
import moment from 'moment';
</script>

<template>
  <div class="accordion py-4" id="log-list">
    <div class="accordion-item" v-for="(item, i) in items" :key="i">
      <h2 class="accordion-header" :id="'heading' + i">
        <div
          class="accordion-button row collapsed p-0 pe-3 ms-0"
          type="button"
          data-bs-toggle="collapse"
          :data-bs-target="'#item' + i"
          aria-expanded="false"
          :aria-controls="'item' + i"
        >
          <span class="type-color" :class="getTypeColor(item.level)"></span>
          <div class="col-md-2 col-sm-12 p-3 timestamp">
            {{ timestamp(item.timestamp) }}
          </div>
          <div class="col p-3">
            {{ limitString(item.messageTemplate, 150) }}
          </div>
        </div>
      </h2>
      <div
        :id="'item' + i"
        class="accordion-collapse collapse"
        aria-labelledby="headingThree"
        data-bs-parent="#log-list"
      >
        <div class="accordion-body" v-html="item.exception"></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    items: {
      type: Array,
      default: () => [],
    },
  },
  methods: {
    limitString(str, maxLength = 100) {
      if (str.length < maxLength) return str;
      return str.substr(0, maxLength) + '...';
    },
    getTypeColor(type) {
      return 'level-' + type.toLowerCase();
    },
    timestamp(str) {
      return moment(str).format('YYYY-MM-DD HH:mm:SS');
    },
  },
};
</script>
