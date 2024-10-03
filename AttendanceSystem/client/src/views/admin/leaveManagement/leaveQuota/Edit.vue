<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Leave Quota</span>
        </v-card-title>

        <v-card-text>
            <v-snackbar :color="snackbar.color"
                        top
                        v-model="snackbar.snackbar">
                {{ snackbar.message }}
                <v-btn @click="snackbar.snackbar = false"
                       dark
                       text>
                    Close
                </v-btn>
            </v-snackbar>
            <v-container>
                <v-simple-table fixed-header height="500px">
                    <template v-slot:default>
                        <thead>
                        <tr>
                            <th class="text-left">Leave</th>
                            <th class="text-left">Balance</th>
                            <th class="text-left">Leave Increment Period</th>
                            <th class="text-left">Applicable Gender</th>
                            <th class="text-left">Is Paid Leave</th>
                            <th class="text-left">Is Leave Carry able</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr v-for="(item,index) in formData" :key="index">
                            <td>{{item.leaveType}}</td>
                            <td>
                                <v-text-field
                                        type="number"
                                        v-model.number="formData[index].leaveBalance"
                                ></v-text-field>
                            </td>
                            <td>{{item.leaveIncrementPeroid}}</td>
                            <td>{{item.applicableGender}}</td>
                            <td>
                                <v-checkbox v-model="formData[index].isPaidLeave" color="teal" class="mx-2"></v-checkbox>
                            </td>
                            <td>
                                <v-checkbox v-model="formData[index].isLeaveCarryable" color="teal" class="mx-2"></v-checkbox>
                            </td>
                        </tr>
                        </tbody>
                    </template>
                </v-simple-table>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red darken-1" text @click="closeForm">Cancel</v-btn>
            <v-btn color="teal darken-1" text @click="saveData">Save</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        name: "LeaveQuotaEdit",
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
                formData: [],
                submitStatus: ''
            }
        },
        methods: {
            closeForm() {
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
                this.$v.$reset()
            },
            async saveData() {
                    try {
                        const {data} = await axios.post('leaveQuota/CreateLeaveQuota', this.formData)
                        this.snackbar.message = data.success ? 'Data Updated Successfully' : data.errors.join(', ')
                        this.snackbar.color = data.success ? 'success' : 'error'
                        this.snackbar.data = data
                        if (data.success) {
                            this.closeForm()
                        } else {
                            this.snackbar.snackbar = true
                        }
                    } catch (e) {
                        this.snackbar.message = e
                        this.snackbar.color = 'error'
                        this.snackbar.data = e
                        this.snackbar.snackbar = true
                    }
            },
            async getEditData() {
                try {
                    const {data} = await axios.post(this.ItemDetailUrl, {})
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