import { Component, Input } from '@angular/core'

@Component({
    selector: 'keywords-strip',
    templateUrl: 'keywords-strip.component.html',
    styleUrls: ['./keywords-strip.component.css']
})

export class KeywordsStripComponent {
    @Input() keywords: string

    formatedKeywords() {
      return this.keywords.replace(/,/g, ', ')
    }
}
