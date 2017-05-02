import { Component, OnInit } from '@angular/core'
import {INewsItem} from '../../shared/interfaces'
import { DataService } from '../../shared/data_services/data.service'
import { MaterialModule } from '@angular/material'

@Component({
    selector: 'news',
    templateUrl: './news.component.html',
    styleUrls: ['../../styles.css']

})
export class NewsComponent implements OnInit {
    newsitems: INewsItem[]
    selectedNewsitem: INewsItem

    constructor(private dataService: DataService) { }

    ngOnInit() {
    this.dataService.getNewsItems().subscribe((newsitems: INewsItem[]) => {
        this.newsitems = newsitems
    })
}

    onSelect(newsitem: INewsItem): void {
        this.selectedNewsitem = newsitem
    }
}

