﻿import {
    Component,
    AfterViewInit,
    EventEmitter,
    OnDestroy,
    Input,
    Output,
} from '@angular/core'

import { ConfigService } from '../../_services'

declare const tinymce: any

@Component({
    selector: 'text-editor',
    templateUrl: './texteditor.component.html',
    styleUrls: ['./texteditor.component.css']
})
export class TextEditorComponent implements AfterViewInit, OnDestroy {
    ClientSide: boolean
    TinyMCELoaded: boolean
    Editor: any
    Saved: boolean

    @Input() elementId: string
    @Input() text: string
    @Input() inline: boolean
    @Input() height: number
    @Output() onSubmit = new EventEmitter<string>()
    @Output() onEditorContentChange = new EventEmitter<string>()
    @Output() isSaved = new EventEmitter<boolean>()

    constructor(private configService: ConfigService) {
        this.ClientSide = typeof window !== 'undefined'
        this.isSaved.emit(true)
        this.Saved = true
    }

    handleSubmit() {
        this.isSaved.emit(true)
        this.Saved = true
        const content = this.Editor.getContent()
        this.onSubmit.emit(content)
    }

    ngAfterViewInit() {
        if (this.ClientSide) {
            require.ensure([
                'tinymce'
            ], require => {
                require('tinymce')
                require('tinymce/themes/modern')
                require('tinymce/plugins/table')
                require('tinymce/plugins/link')
                require('tinymce/plugins/lists')
                require('tinymce/plugins/image')
                require('tinymce/plugins/imagetools')
                require('tinymce/plugins/wordcount')

                this.TinyMCELoaded = true

                tinymce.init({
                    selector: '#' + this.elementId,
                    height: this.height || 250,
                    inline: this.inline,
                    plugins: [ 'link', 'table', 'lists', 'image', 'imagetools', 'wordcount' ],
                    toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | media | codesample help',
                    menubar: false,
                    images_upload_url: '/upload',
                    automatic_uploads: true,
                    file_browser_callback_types: 'image',
                    file_picker_types: 'image',
                    file_picker_callback: function (cb, value, meta) {
                        const input = document.createElement('input')
                        input.setAttribute('type', 'file')
                        input.setAttribute('accept', 'image/*')

                        // Note: In modern browsers input[type="file"] is functional without
                        // even adding it to the DOM, but that might not be the case in some older
                        // or quirky browsers like IE, so you might want to add it to the DOM
                        // just in case, and visually hide it. And do not forget do remove it
                        // once you do not need it anymore.

                        input.onchange = function () {
                            const file = this['files'][0]

                            const reader = new FileReader()
                            reader.readAsDataURL(file)
                            reader.onload = function () {
                                // Note: Now we need to register the blob in TinyMCEs image blob
                                // registry. In the next release this part hopefully won't be
                                // necessary, as we are looking to handle it internally.
                                const id = 'blobid' + (new Date()).getTime()
                                const blobCache = tinymce.activeEditor.editorUpload.blobCache
                                const base64 = reader.result.split(',')[1]
                                const blobInfo = blobCache.create(id, file, base64)
                                blobCache.add(blobInfo)

                                // call the callback and populate the Title field with the file name
                                cb(blobInfo.blobUri(), { title: file.name })
                            }
                        }

                        input.click()
                    },
                    save_onsavecallback: () => this.handleSubmit(),
                    skin_url: '/assets/tinymce/skins/lightgray', // Skins is in the wwwroot
                    setup: editor => {
                        this.Editor = editor
                        editor.on('keyup change undo redo', () => {
                            const content = editor.getContent()
                            this.isSaved.emit(false)
                            this.Saved = false
                            this.onEditorContentChange.emit(content)
                        })
                    }
                })
            })
        }
    }

    ngOnDestroy() {
        if (this.ClientSide) {
            tinymce.remove(this.Editor)
        }
    }
}
