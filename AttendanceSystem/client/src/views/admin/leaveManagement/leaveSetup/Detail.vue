<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Leave Detail</span>
        </v-card-title>
        <v-card-text>
            <v-container>
                <v-row no-gutters>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-text-field label="Leave"
                                      readonly
                                      v-model="formData.leaveName"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-text-field label="Leave Code"
                                      readonly
                                      v-model="formData.leaveCode"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-text-field label="Number Of Days"
                                      readonly
                                      v-model.number="formData.leaveDays"
                                      type="number"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-text-field v-model="formData.leaveIncrement"
                                      label="Leave Increment"
                                      readonly
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-text-field v-model="formData.applicableGender"
                                      label="Applicable Gender"
                                      readonly
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="12" class="ma-0 px-2">
                        <v-textarea
                                name="input-7-4"
                                label="Description"
                                readonly
                                v-model="formData.description"
                        ></v-textarea>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-switch v-model="formData.isReplacementLeave"
                                  label="Is Replacement Leave?"
                                  readonly
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-switch v-model="formData.isPaidLeave"
                                  label="Is Paid Leave?"
                                  readonly
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="6" class="ma-0 px-2">
                        <v-switch v-model="formData.isLeaveCarryable"
                                  label="Is Leave Carry able?"
                                  readonly
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