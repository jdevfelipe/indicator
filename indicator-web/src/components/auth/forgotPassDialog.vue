<template>
  <div>
    <q-dialog v-model="show" persistent>
      <q-card style="min-width: 350px">
        <q-card-section>
          <div class="text-h6">Seu login</div>
        </q-card-section>

        <q-card-section>
          <div>
            Digite seu cpf e enviaremos sua senha para o email que vc usou para
            criar a conta conosco!
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none flex justify-center">
          <q-input
            dense
            v-model="cpf"
            mask="###.###.###-##"
            autofocus
            @keyup.enter="prompt = false"
            style="width: 250px"
          />
        </q-card-section>

        <q-card-actions align="right" class="text-primary">
          <q-btn flat label="fechar" @click="$emit('close')" />
          <q-btn flat label="enviar" @click="forgotPassRequest" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import AuthService from "../../services/auth.service";
import NotifyService from "../../utils/notify.service";

const authService = new AuthService();
const notifyService = new NotifyService();
export default {
  name: "ForgotPassDialog",
  props: ["show"],
  data() {
    return {
      cpf: "",
    };
  },
  methods: {
    forgotPassRequest() {
      if (!this.cpf) {
        notifyService.notify("Preencha o campo acima!", "negative")
        return
      }

      this.cpf = this.cpf.replace(/\D/g,'');
      authService.forgotPassRequest(this.cpf).then((res) => {
        if (res.data) 
        {
          notifyService.notify("Tudo certo, olhe seu email!", "positive")
        } else {
          notifyService.notify("Algo deu errado, tente mais tarde", "negative")
        }
      }).catch(e => {
        console.log(e);
        notifyService.notify(e.response.data.error, "negative")
      });
    },
  },
};
</script>
