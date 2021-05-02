<template>
  <q-dialog v-model="show" persistent>
    <q-card style="min-width: 350px">
      <q-card-section>
        <div class="text-h6">Your address</div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-input
          dense
          v-model="user.name"
          autofocus
          @keyup.enter="prompt = false"
        />
      </q-card-section>
      <q-card-section class="q-pt-none">
        <q-input
          dense
          v-model="user.cpf"
          disable
          autofocus
          @keyup.enter="prompt = false"
        />
      </q-card-section>
      <q-card-section class="q-pt-none">
        <q-input
          dense
          v-model="user.email"
          autofocus
          @keyup.enter="prompt = false"
        />
      </q-card-section>
      <q-card-section class="q-pt-none">
        <q-select
          outlined
          v-model="role"
          :options="optionsRole"
          map-options
          emit-value
          label="Standard"
        />
      </q-card-section>
            <q-card-section class="q-pt-none">
        <q-select
          outlined
          v-model="status"
          :options="optionsStatus"
          map-options
          emit-value
          label="Standard"
        />
      </q-card-section>

      <q-card-actions align="right" class="text-primary">
        <q-btn flat label="Cancelar" @click="$emit('close')" />
        <q-btn flat label="SALVAR" @click="edit()" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
import CompaniesService from "../../services/companies.service";
import NotifyService from "../../utils/notify.service";

const companiesService = new CompaniesService();
const notifyService = new NotifyService();
export default {
  name: "userDialog",
  props: ["show", "user"],
  data() {
    return {
      maximizedToggle: true,
      optionsRole: [
        {
          label: "ANALISTA",
          value: 2,
        },
        {
          label: "GERENTE DE CONTA",
          value: 1,
        },
        {
          label: "USUÁRIO",
          value: 0,
        },
      ],
      role: "",
      optionsStatus: [
        {
          label: "BLOQUEADO",
          value: 2,
        },
        {
          label: "INATIVO",
          value: 1,
        },
        {
          label: "ATIVO",
          value: 0,
        },
      ],
      status: "",
    };
  },
  mounted() {
    let vm = this;
    vm.role = vm.user.userRole;
    vm.status = vm.user.userEnum;
  },
  methods: {
    edit() {
      this.user.userEnum = this.status
      this.$q.loading.show();
      companiesService
        .updateUser(this.user)
        .then((res) => {
          this.$q.loading.hide();
          this.$emit("updateList");
          notifyService.notify("Alterado com sucesso!", "positive");
          this.$emit("close");
        })
        .catch((e) => {
          notifyService.notify("Algo deu errado", "warning");
        });
    },
  },
};
</script>
