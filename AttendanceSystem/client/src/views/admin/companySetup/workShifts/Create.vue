<template>
    <v-card class="customFontSize">
        <v-card-title>
            <span class="headline">Add Work Shift</span>
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
                <v-row>
                    <v-col cols="12" md="3">
                        <v-text-field label="Code"
                                      v-model="formData.shiftCode"
                                      :error-messages="shiftCodeErrors"
                                      @input="$v.formData.shiftCode.$touch()"
                                      @blur="$v.formData.shiftCode.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-text-field label="Name"
                                      v-model="formData.shiftName"
                                      :error-messages="shiftNameErrors"
                                      @input="$v.formData.shiftName.$touch()"
                                      @blur="$v.formData.shiftName.$touch()"
                        ></v-text-field>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Shift Start"
                                v-model="formData.shiftStart"
                                prepend-icon="access_time"
                                class="text-subtitle-1"
                                small
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Shift End"
                                v-model="formData.shiftEnd"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Beginning In"
                                v-model="formData.beginingIn"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Ending In"
                                v-model="formData.endingIn"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Beginning Out"
                                v-model="formData.beginingOut"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Ending Out"
                                v-model="formData.endingOut"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Late In"
                                v-model="formData.lateIn"
                                prepend-icon="access_time"
                        />
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-digital-time-picker
                                label="Leave Early"
                                v-model="formData.leaveEarly"
                                prepend-icon="access_time"
                        />
                    </v-col>
                </v-row>
                <v-card color="teal lighten-5">
                    <v-card-text v-if="formData.shiftList.length">
                        <v-row v-for="(item,index) in formData.shiftList" :key="index">
                            <v-col cols="12" md="3">
                                <v-digital-time-picker
                                        v-model="formData.shiftList[index].startTime"
                                        :label="`${item.name}  Start Time`"
                                        prepend-icon="access_time"
                                />
                            </v-col>
                            <v-col cols="12" md="3">
                                <v-digital-time-picker
                                        v-model="formData.shiftList[index].endTime"
                                        :label="`${item.name}  End Time`"
                                        prepend-icon="access_time"
                                />
                            </v-col>
                            <v-col cols="12" md="3">
                                <v-text-field v-model.number="formData.shiftList[index].count"
                                              type="number"
                                              :label="`${item.name}  Count`"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12" md="3">
                                <v-text-field v-model.number="formData.shiftList[index].duration"
                                              :label="`${item.name}  Duration in minute`"
                                              type="number"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
                <v-card flat>
                    <v-card-actions>
                        <v-combobox multiple
                                    v-model="snackbar.select"
                                    label="Add Field if Necessary"
                                    chips
                                    dense
                                    append-icon=""
                                    color="teal"
                                    deletable-chips
                                    class="tag-input"
                                    :append-outer-icon="'mdi-send'"
                                    @click:append-outer="addField"
                        >
                        </v-combobox>
                    </v-card-actions>
                </v-card>
                <v-row>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isMustCheckIn"
                                  label="Must Check In ?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isMustCheckOut"
                                  label="Must Check Out ?"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isLateIn"
                                  label="Late In"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                    <v-col cols="12" md="3">
                        <v-switch v-model="formData.isEarlyLeave"
                                  label="Early Leave"
                                  clearable
                                  color="teal"
                        ></v-switch>
                    </v-col>
                </v-row>
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
    import {required} from "vuelidate/lib/validators";
    import axios from 'axios'

    export default {
        name: "WorkShiftCreate",
        computed: {
            shiftNameErrors() {
                const errors = []
                if (!this.$v.formData.shiftName.$dirty) return errors
                !this.$v.formData.shiftName.required && errors.push('Work shift Name is required.')
                return errors
            },
            shiftCodeErrors() {
                const errors = []
                if (!this.$v.formData.shiftCode.$dirty) return errors
                !this.$v.formData.shiftCode.required && errors.push('Work shift Code is required.')
                return errors
            },
        },
        data() {
            return {
                snackbar: {
                    snackbar:false,
                    loopMenuStartTime: [false],
                    loopMenuEndTime: [false],
                    data: {},
                    message: '',
                    color: '',
                    select: []
                },
                formData: {
                    shiftList: []
                },
                tempFilteredArray:[],
                submitStatus: ''
            }
        },
        methods: {
            closeForm() {
                this.$v.$reset()
                if(this.snackbar.color === 'error'){
                    this.snackbar.message = ''
                }
                this.$emit("closeDialog", this.snackbar)
            },
            async saveData() {
                this.$v.$touch()
                if (this.$v.$invalid) {
                    this.submitStatus = 'ERROR'
                } else {
                    try {
                        const {data} = await axios.post('WorkShift/InsertIntoWorkShiftAsync', this.formData)
                        this.snackbar.message = data.success ? 'Work Shift  Created Successfully' : data.errors.join(', ')
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
                }

            },
            addField() {
               if(this.snackbar.select.length){
                   this.snackbar.select.forEach(item => {
                       console.log('inside loop')
                       if (this.formData.shiftList.filter(e => e.name === item).length <= 0) {
                           console.log('inside if')
                           this.formData.shiftList.push({
                               name: item,
                               startTime: '',
                               endTime: '',
                               count: '',
                               duration: '',
                           })

                           /* for(let i=0;i<this.formData.shiftList.length;i++){
                                if(!this.snackbar.select.contains(this.formData.shiftList[i].name)){

                                }
                            }*/

                       }
                       this.tempFilteredArray = []
                       this.tempFilteredArray = this.formData.shiftList.filter((item) => {
                           return this.snackbar.select.includes(item.name)
                       })
                   })
                   this.formData.shiftList = this.tempFilteredArray
               }else{
                   this.formData.shiftList = []
               }

            }
        },
        created() {

        },
        validations: {
            formData: {
                shiftName: {required},
                shiftCode: {required},
            }
        },
        components: {}
    }
</script>

<style scoped lang="scss">
    ::v-deep.tag-input span.v-chip {
        background-color: teal;
        color: #fff;
        font-size: 1em;
        padding-left: 7px;
    }
</style>