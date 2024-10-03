<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Device Detail</span>
        </v-card-title>
        <v-card-text>
            <v-container>
                <v-row no-gutters>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Device"
                                      readonly
                                      v-model="formData.deviceName"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Device Number"
                                      readonly
                                      v-model="formData.deviceNumber"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Serial Number"
                                      readonly
                                      v-model="formData.serialNumber"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Device Model"
                                      readonly
                                      v-model="formData.deviceModelID"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="IP Address"
                                      readonly
                                      v-model="formData.ipAddress"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Communication Port"
                                      readonly
                                      v-model.number="formData.communicationPort"
                                      type="number"
                        ></v-text-field>
                    </v-col>
                    <!--<v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Serial Port"
                                      readonly
                                      v-model.number="formData.serialPort"
                                      type="number"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Terminal IP"
                                      readonly
                                      v-model="formData.terminalIP"
                        ></v-text-field>
                    </v-col>-->
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Communication Password"
                                      readonly
                                      v-model="formData.communicationPassword"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-text-field label="Communication Mode"
                                      readonly
                                      v-model="formData.communicationMode"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-switch v-model="formData.isActive"
                                  label="Is Active?"
                                  readonly
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6" class="px-2 ma-0">
                        <v-switch v-model="formData.isRegister"
                                  label="Is Registered?"
                                  readonly
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                </v-row>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red darken-1" text @click="closeForm">Close</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "DetailGrid",
        props: {
            getItemUrl: {
                type: String,
                required: true
            }
        },
        computed: {
            ItemDetailUrl() {
                return this.getItemUrl
            },
        },
        data() {
            return {
                snackbar: {
                    data: {},
                    message: '',
                    color: '',
                    snackbar: false
                },
                formData: {}
            }
        },
        methods: {
            closeForm() {
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
            },
            async getEditData() {
                try {
                    const {data} = await axios.get(this.ItemDetailUrl)
                    this.formData = data
                } catch (e) {
                    console.log(e)
                }

            }
        },
        created() {
            this.getEditData()
        },
    }
</script>

<style scoped>

</style>