import { LitElement, css, html, nothing } from "lit";
import { customElement, property } from "lit/decorators.js";
import { unsafeHTML } from "lit/directives/unsafe-html.js";
import { until } from 'lit/directives/until.js';

import DOMPurify from "dompurify";
import { marked } from "marked";

@customElement('marked-block')
export class MarkedBlock extends LitElement {

  @property({ type: String })
  value?: string;

  render() {
    if (!this.value) return nothing;

    return until(
      this.parse(this.value),
      html`<div>...</div>`);
  }

  private parse = async (value: string) => {
    value = value.replace(/^[\u200B\u200C\u200D\u200E\u200F\uFEFF]/,"");
    value = await marked.parse(value, {
      async: true,
      gfm: true,
    });
    value = DOMPurify.sanitize(value);
    return html`
      <div class="markdown-body">
        ${unsafeHTML(value)}
      </div>
    `;
  }

  static styles = css`
    :host {
      display: block;
      width: 100%;

      --base-size-4: 0.25rem;
      --base-size-8: 0.5rem;
      --base-size-16: 1rem;
      --base-size-24: 1.5rem;
      --base-size-40: 2.5rem;
      --base-text-weight-normal: 400;
      --base-text-weight-medium: 500;
      --base-text-weight-semibold: 600;
      --fontStack-monospace: ui-monospace, SFMono-Regular, SF Mono, Menlo, Consolas, Liberation Mono, monospace;
      --fgColor-accent: Highlight;
    }

    .markdown-body {
      -ms-text-size-adjust: 100%;
      -webkit-text-size-adjust: 100%;
      margin: 0;
      color: var(--fgColor-default);
      background-color: transparent;
      font-family: -apple-system,BlinkMacSystemFont,"Segoe UI","Noto Sans",Helvetica,Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji";
      font-size: 16px;
      line-height: 1.5;
      word-wrap: break-word;
    }

    .markdown-body .octicon {
      display: inline-block;
      fill: currentColor;
      vertical-align: text-bottom;
    }

    .markdown-body h1:hover .anchor .octicon-link:before,
    .markdown-body h2:hover .anchor .octicon-link:before,
    .markdown-body h3:hover .anchor .octicon-link:before,
    .markdown-body h4:hover .anchor .octicon-link:before,
    .markdown-body h5:hover .anchor .octicon-link:before,
    .markdown-body h6:hover .anchor .octicon-link:before {
      width: 16px;
      height: 16px;
      content: ' ';
      display: inline-block;
      background-color: currentColor;
      -webkit-mask-image: url("data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' version='1.1' aria-hidden='true'><path fill-rule='evenodd' d='M7.775 3.275a.75.75 0 001.06 1.06l1.25-1.25a2 2 0 112.83 2.83l-2.5 2.5a2 2 0 01-2.83 0 .75.75 0 00-1.06 1.06 3.5 3.5 0 004.95 0l2.5-2.5a3.5 3.5 0 00-4.95-4.95l-1.25 1.25zm-4.69 9.64a2 2 0 010-2.83l2.5-2.5a2 2 0 012.83 0 .75.75 0 001.06-1.06 3.5 3.5 0 00-4.95 0l-2.5 2.5a3.5 3.5 0 004.95 4.95l1.25-1.25a.75.75 0 00-1.06-1.06l-1.25 1.25a2 2 0 01-2.83 0z'></path></svg>");
      mask-image: url("data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' version='1.1' aria-hidden='true'><path fill-rule='evenodd' d='M7.775 3.275a.75.75 0 001.06 1.06l1.25-1.25a2 2 0 112.83 2.83l-2.5 2.5a2 2 0 01-2.83 0 .75.75 0 00-1.06 1.06 3.5 3.5 0 004.95 0l2.5-2.5a3.5 3.5 0 00-4.95-4.95l-1.25 1.25zm-4.69 9.64a2 2 0 010-2.83l2.5-2.5a2 2 0 012.83 0 .75.75 0 001.06-1.06 3.5 3.5 0 00-4.95 0l-2.5 2.5a3.5 3.5 0 004.95 4.95l1.25-1.25a.75.75 0 00-1.06-1.06l-1.25 1.25a2 2 0 01-2.83 0z'></path></svg>");
    }

    .markdown-body details,
    .markdown-body figcaption,
    .markdown-body figure {
      display: block;
    }

    .markdown-body summary {
      display: list-item;
    }

    .markdown-body [hidden] {
      display: none !important;
    }

    .markdown-body a {
      background-color: transparent;
      color: var(--fgColor-accent);
      text-decoration: none;
    }

    .markdown-body abbr[title] {
      border-bottom: none;
      -webkit-text-decoration: underline dotted;
      text-decoration: underline dotted;
    }

    .markdown-body b,
    .markdown-body strong {
      font-weight: var(--base-text-weight-semibold, 600);
    }

    .markdown-body dfn {
      font-style: italic;
    }

    .markdown-body h1 {
      margin: .67em 0;
      font-weight: var(--base-text-weight-semibold, 600);
      padding-bottom: .3em;
      font-size: 2em;
      border-bottom: 1px solid var(--borderColor-muted);
    }

    .markdown-body mark {
      background-color: var(--bgColor-attention-muted);
      color: var(--fgColor-default);
    }

    .markdown-body small {
      font-size: 90%;
    }

    .markdown-body sub,
    .markdown-body sup {
      font-size: 75%;
      line-height: 0;
      position: relative;
      vertical-align: baseline;
    }

    .markdown-body sub {
      bottom: -0.25em;
    }

    .markdown-body sup {
      top: -0.5em;
    }

    .markdown-body img {
      border-style: none;
      max-width: 100%;
      box-sizing: content-box;
    }

    .markdown-body code,
    .markdown-body kbd,
    .markdown-body pre,
    .markdown-body samp {
      font-family: monospace;
      font-size: 1em;
    }

    .markdown-body figure {
      margin: 1em var(--base-size-40);
    }

    .markdown-body hr {
      box-sizing: content-box;
      overflow: hidden;
      background: transparent;
      border-bottom: 1px solid var(--borderColor-muted);
      height: .25em;
      padding: 0;
      margin: var(--base-size-24) 0;
      background-color: transparent;
      border: 0;
    }

    .markdown-body input {
      font: inherit;
      margin: 0;
      overflow: visible;
      font-family: inherit;
      font-size: inherit;
      line-height: inherit;
    }

    .markdown-body [type=button],
    .markdown-body [type=reset],
    .markdown-body [type=submit] {
      -webkit-appearance: button;
      appearance: button;
    }

    .markdown-body [type=checkbox],
    .markdown-body [type=radio] {
      box-sizing: border-box;
      padding: 0;
    }

    .markdown-body [type=number]::-webkit-inner-spin-button,
    .markdown-body [type=number]::-webkit-outer-spin-button {
      height: auto;
    }

    .markdown-body [type=search]::-webkit-search-cancel-button,
    .markdown-body [type=search]::-webkit-search-decoration {
      -webkit-appearance: none;
      appearance: none;
    }

    .markdown-body ::-webkit-input-placeholder {
      color: inherit;
      opacity: .54;
    }

    .markdown-body ::-webkit-file-upload-button {
      -webkit-appearance: button;
      appearance: button;
      font: inherit;
    }

    .markdown-body a:hover {
      text-decoration: underline;
    }

    .markdown-body ::placeholder {
      color: var(--fgColor-muted);
      opacity: 1;
    }

    .markdown-body hr::before {
      display: table;
      content: "";
    }

    .markdown-body hr::after {
      display: table;
      clear: both;
      content: "";
    }

    .markdown-body table {
      border-spacing: 0;
      border-collapse: collapse;
      display: block;
      width: max-content;
      max-width: 100%;
      overflow: auto;
      font-variant: tabular-nums;
    }

    .markdown-body td,
    .markdown-body th {
      padding: 0;
    }

    .markdown-body details summary {
      cursor: pointer;
    }

    .markdown-body a:focus,
    .markdown-body [role=button]:focus,
    .markdown-body input[type=radio]:focus,
    .markdown-body input[type=checkbox]:focus {
      outline: 2px solid var(--focus-outlineColor);
      outline-offset: -2px;
      box-shadow: none;
    }

    .markdown-body a:focus:not(:focus-visible),
    .markdown-body [role=button]:focus:not(:focus-visible),
    .markdown-body input[type=radio]:focus:not(:focus-visible),
    .markdown-body input[type=checkbox]:focus:not(:focus-visible) {
      outline: solid 1px transparent;
    }

    .markdown-body a:focus-visible,
    .markdown-body [role=button]:focus-visible,
    .markdown-body input[type=radio]:focus-visible,
    .markdown-body input[type=checkbox]:focus-visible {
      outline: 2px solid var(--focus-outlineColor);
      outline-offset: -2px;
      box-shadow: none;
    }

    .markdown-body a:not([class]):focus,
    .markdown-body a:not([class]):focus-visible,
    .markdown-body input[type=radio]:focus,
    .markdown-body input[type=radio]:focus-visible,
    .markdown-body input[type=checkbox]:focus,
    .markdown-body input[type=checkbox]:focus-visible {
      outline-offset: 0;
    }

    .markdown-body kbd {
      display: inline-block;
      padding: var(--base-size-4);
      font: 11px var(--fontStack-monospace, ui-monospace, SFMono-Regular, SF Mono, Menlo, Consolas, Liberation Mono, monospace);
      line-height: 10px;
      color: var(--fgColor-default);
      vertical-align: middle;
      background-color: var(--bgColor-muted);
      border: solid 1px var(--borderColor-neutral-muted);
      border-bottom-color: var(--borderColor-neutral-muted);
      border-radius: 6px;
      box-shadow: inset 0 -1px 0 var(--borderColor-neutral-muted);
    }

    .markdown-body h1,
    .markdown-body h2,
    .markdown-body h3,
    .markdown-body h4,
    .markdown-body h5,
    .markdown-body h6 {
      margin-top: var(--base-size-24);
      margin-bottom: var(--base-size-16);
      font-weight: var(--base-text-weight-semibold, 600);
      line-height: 1.25;
    }

    .markdown-body h2 {
      font-weight: var(--base-text-weight-semibold, 600);
      padding-bottom: .3em;
      font-size: 1.5em;
      border-bottom: 1px solid var(--borderColor-muted);
    }

    .markdown-body h3 {
      font-weight: var(--base-text-weight-semibold, 600);
      font-size: 1.25em;
    }

    .markdown-body h4 {
      font-weight: var(--base-text-weight-semibold, 600);
      font-size: 1em;
    }

    .markdown-body h5 {
      font-weight: var(--base-text-weight-semibold, 600);
      font-size: .875em;
    }

    .markdown-body h6 {
      font-weight: var(--base-text-weight-semibold, 600);
      font-size: .85em;
      color: var(--fgColor-muted);
    }

    .markdown-body p {
      margin-top: 0;
      margin-bottom: 10px;
    }

    .markdown-body blockquote {
      margin: 0;
      padding: 0 1em;
      color: var(--fgColor-muted);
      border-left: .25em solid var(--borderColor-default);
    }

    .markdown-body ul,
    .markdown-body ol {
      margin-top: 0;
      margin-bottom: 0;
      padding-left: 2em;
    }

    .markdown-body ol ol,
    .markdown-body ul ol {
      list-style-type: lower-roman;
    }

    .markdown-body ul ul ol,
    .markdown-body ul ol ol,
    .markdown-body ol ul ol,
    .markdown-body ol ol ol {
      list-style-type: lower-alpha;
    }

    .markdown-body dd {
      margin-left: 0;
    }

    .markdown-body tt,
    .markdown-body code,
    .markdown-body samp {
      font-family: var(--fontStack-monospace, ui-monospace, SFMono-Regular, SF Mono, Menlo, Consolas, Liberation Mono, monospace);
      font-size: 12px;
    }

    .markdown-body pre {
      margin-top: 0;
      margin-bottom: 0;
      font-family: var(--fontStack-monospace, ui-monospace, SFMono-Regular, SF Mono, Menlo, Consolas, Liberation Mono, monospace);
      font-size: 12px;
      word-wrap: normal;
    }

    .markdown-body .octicon {
      display: inline-block;
      overflow: visible !important;
      vertical-align: text-bottom;
      fill: currentColor;
    }

    .markdown-body input::-webkit-outer-spin-button,
    .markdown-body input::-webkit-inner-spin-button {
      margin: 0;
      appearance: none;
    }

    .markdown-body .mr-2 {
      margin-right: var(--base-size-8, 8px) !important;
    }

    .markdown-body::before {
      display: table;
      content: "";
    }

    .markdown-body::after {
      display: table;
      clear: both;
      content: "";
    }

    .markdown-body>*:first-child {
      margin-top: 0 !important;
    }

    .markdown-body>*:last-child {
      margin-bottom: 0 !important;
    }

    .markdown-body a:not([href]) {
      color: inherit;
      text-decoration: none;
    }

    .markdown-body .absent {
      color: var(--fgColor-danger);
    }

    .markdown-body .anchor {
      float: left;
      padding-right: var(--base-size-4);
      margin-left: -20px;
      line-height: 1;
    }

    .markdown-body .anchor:focus {
      outline: none;
    }

    .markdown-body p,
    .markdown-body blockquote,
    .markdown-body ul,
    .markdown-body ol,
    .markdown-body dl,
    .markdown-body table,
    .markdown-body pre,
    .markdown-body details {
      margin-top: 0;
      margin-bottom: var(--base-size-16);
    }

    .markdown-body blockquote>:first-child {
      margin-top: 0;
    }

    .markdown-body blockquote>:last-child {
      margin-bottom: 0;
    }

    .markdown-body h1 .octicon-link,
    .markdown-body h2 .octicon-link,
    .markdown-body h3 .octicon-link,
    .markdown-body h4 .octicon-link,
    .markdown-body h5 .octicon-link,
    .markdown-body h6 .octicon-link {
      color: var(--fgColor-default);
      vertical-align: middle;
      visibility: hidden;
    }

    .markdown-body h1:hover .anchor,
    .markdown-body h2:hover .anchor,
    .markdown-body h3:hover .anchor,
    .markdown-body h4:hover .anchor,
    .markdown-body h5:hover .anchor,
    .markdown-body h6:hover .anchor {
      text-decoration: none;
    }

    .markdown-body h1:hover .anchor .octicon-link,
    .markdown-body h2:hover .anchor .octicon-link,
    .markdown-body h3:hover .anchor .octicon-link,
    .markdown-body h4:hover .anchor .octicon-link,
    .markdown-body h5:hover .anchor .octicon-link,
    .markdown-body h6:hover .anchor .octicon-link {
      visibility: visible;
    }

    .markdown-body h1 tt,
    .markdown-body h1 code,
    .markdown-body h2 tt,
    .markdown-body h2 code,
    .markdown-body h3 tt,
    .markdown-body h3 code,
    .markdown-body h4 tt,
    .markdown-body h4 code,
    .markdown-body h5 tt,
    .markdown-body h5 code,
    .markdown-body h6 tt,
    .markdown-body h6 code {
      padding: 0 .2em;
      font-size: inherit;
    }

    .markdown-body summary h1,
    .markdown-body summary h2,
    .markdown-body summary h3,
    .markdown-body summary h4,
    .markdown-body summary h5,
    .markdown-body summary h6 {
      display: inline-block;
    }

    .markdown-body summary h1 .anchor,
    .markdown-body summary h2 .anchor,
    .markdown-body summary h3 .anchor,
    .markdown-body summary h4 .anchor,
    .markdown-body summary h5 .anchor,
    .markdown-body summary h6 .anchor {
      margin-left: -40px;
    }

    .markdown-body summary h1,
    .markdown-body summary h2 {
      padding-bottom: 0;
      border-bottom: 0;
    }

    .markdown-body ul.no-list,
    .markdown-body ol.no-list {
      padding: 0;
      list-style-type: none;
    }

    .markdown-body ol[type="a s"] {
      list-style-type: lower-alpha;
    }

    .markdown-body ol[type="A s"] {
      list-style-type: upper-alpha;
    }

    .markdown-body ol[type="i s"] {
      list-style-type: lower-roman;
    }

    .markdown-body ol[type="I s"] {
      list-style-type: upper-roman;
    }

    .markdown-body ol[type="1"] {
      list-style-type: decimal;
    }

    .markdown-body div>ol:not([type]) {
      list-style-type: decimal;
    }

    .markdown-body ul ul,
    .markdown-body ul ol,
    .markdown-body ol ol,
    .markdown-body ol ul {
      margin-top: 0;
      margin-bottom: 0;
    }

    .markdown-body li>p {
      margin-top: var(--base-size-16);
    }

    .markdown-body li+li {
      margin-top: .25em;
    }

    .markdown-body dl {
      padding: 0;
    }

    .markdown-body dl dt {
      padding: 0;
      margin-top: var(--base-size-16);
      font-size: 1em;
      font-style: italic;
      font-weight: var(--base-text-weight-semibold, 600);
    }

    .markdown-body dl dd {
      padding: 0 var(--base-size-16);
      margin-bottom: var(--base-size-16);
    }

    .markdown-body table th {
      font-weight: var(--base-text-weight-semibold, 600);
    }

    .markdown-body table th,
    .markdown-body table td {
      padding: 6px 13px;
      border: 1px solid var(--borderColor-default);
    }

    .markdown-body table td>:last-child {
      margin-bottom: 0;
    }

    .markdown-body table tr {
      background-color: var(--bgColor-default);
      border-top: 1px solid var(--borderColor-muted);
    }

    .markdown-body table tr:nth-child(2n) {
      background-color: var(--bgColor-muted);
    }

    .markdown-body table img {
      background-color: transparent;
    }

    .markdown-body img[align=right] {
      padding-left: 20px;
    }

    .markdown-body img[align=left] {
      padding-right: 20px;
    }

    .markdown-body .emoji {
      max-width: none;
      vertical-align: text-top;
      background-color: transparent;
    }

    .markdown-body span.frame {
      display: block;
      overflow: hidden;
    }

    .markdown-body span.frame>span {
      display: block;
      float: left;
      width: auto;
      padding: 7px;
      margin: 13px 0 0;
      overflow: hidden;
      border: 1px solid var(--borderColor-default);
    }

    .markdown-body span.frame span img {
      display: block;
      float: left;
    }

    .markdown-body span.frame span span {
      display: block;
      padding: 5px 0 0;
      clear: both;
      color: var(--fgColor-default);
    }

    .markdown-body span.align-center {
      display: block;
      overflow: hidden;
      clear: both;
    }

    .markdown-body span.align-center>span {
      display: block;
      margin: 13px auto 0;
      overflow: hidden;
      text-align: center;
    }

    .markdown-body span.align-center span img {
      margin: 0 auto;
      text-align: center;
    }

    .markdown-body span.align-right {
      display: block;
      overflow: hidden;
      clear: both;
    }

    .markdown-body span.align-right>span {
      display: block;
      margin: 13px 0 0;
      overflow: hidden;
      text-align: right;
    }

    .markdown-body span.align-right span img {
      margin: 0;
      text-align: right;
    }

    .markdown-body span.float-left {
      display: block;
      float: left;
      margin-right: 13px;
      overflow: hidden;
    }

    .markdown-body span.float-left span {
      margin: 13px 0 0;
    }

    .markdown-body span.float-right {
      display: block;
      float: right;
      margin-left: 13px;
      overflow: hidden;
    }

    .markdown-body span.float-right>span {
      display: block;
      margin: 13px auto 0;
      overflow: hidden;
      text-align: right;
    }

    .markdown-body code,
    .markdown-body tt {
      padding: .2em .4em;
      margin: 0;
      font-size: 85%;
      white-space: break-spaces;
      background-color: var(--bgColor-neutral-muted);
      border-radius: 6px;
    }

    .markdown-body code br,
    .markdown-body tt br {
      display: none;
    }

    .markdown-body del code {
      text-decoration: inherit;
    }

    .markdown-body samp {
      font-size: 85%;
    }

    .markdown-body pre code {
      font-size: 100%;
    }

    .markdown-body pre>code {
      padding: 0;
      margin: 0;
      word-break: break-word;
      white-space: pre-wrap;
      background: transparent;
      border: 0;
    }

    .markdown-body .highlight {
      margin-bottom: var(--base-size-16);
    }

    .markdown-body .highlight pre {
      margin-bottom: 0;
      word-break: normal;
    }

    .markdown-body .highlight pre,
    .markdown-body pre {
      padding: var(--base-size-16);
      overflow: auto;
      font-size: 85%;
      line-height: 1.45;
      color: var(--fgColor-default);
      background-color: var(--bgColor-muted);
      border-radius: 6px;
    }

    .markdown-body pre code,
    .markdown-body pre tt {
      display: inline;
      max-width: auto;
      padding: 0;
      margin: 0;
      overflow: visible;
      line-height: inherit;
      word-wrap: normal;
      background-color: transparent;
      border: 0;
    }

    .markdown-body .csv-data td,
    .markdown-body .csv-data th {
      padding: 5px;
      overflow: hidden;
      font-size: 12px;
      line-height: 1;
      text-align: left;
      white-space: nowrap;
    }

    .markdown-body .csv-data .blob-num {
      padding: 10px var(--base-size-8) 9px;
      text-align: right;
      background: var(--bgColor-default);
      border: 0;
    }

    .markdown-body .csv-data tr {
      border-top: 0;
    }

    .markdown-body .csv-data th {
      font-weight: var(--base-text-weight-semibold, 600);
      background: var(--bgColor-muted);
      border-top: 0;
    }

    .markdown-body [data-footnote-ref]::before {
      content: "[";
    }

    .markdown-body [data-footnote-ref]::after {
      content: "]";
    }

    .markdown-body .footnotes {
      font-size: 12px;
      color: var(--fgColor-muted);
      border-top: 1px solid var(--borderColor-default);
    }

    .markdown-body .footnotes ol {
      padding-left: var(--base-size-16);
    }

    .markdown-body .footnotes ol ul {
      display: inline-block;
      padding-left: var(--base-size-16);
      margin-top: var(--base-size-16);
    }

    .markdown-body .footnotes li {
      position: relative;
    }

    .markdown-body .footnotes li:target::before {
      position: absolute;
      top: calc(var(--base-size-8)*-1);
      right: calc(var(--base-size-8)*-1);
      bottom: calc(var(--base-size-8)*-1);
      left: calc(var(--base-size-24)*-1);
      pointer-events: none;
      content: "";
      border: 2px solid var(--borderColor-accent-emphasis);
      border-radius: 6px;
    }

    .markdown-body .footnotes li:target {
      color: var(--fgColor-default);
    }

    .markdown-body .footnotes .data-footnote-backref g-emoji {
      font-family: monospace;
    }

    .markdown-body body:has(:modal) {
      padding-right: var(--dialog-scrollgutter) !important;
    }

    .markdown-body .pl-c {
      color: var(--color-prettylights-syntax-comment);
    }

    .markdown-body .pl-c1,
    .markdown-body .pl-s .pl-v {
      color: var(--color-prettylights-syntax-constant);
    }

    .markdown-body .pl-e,
    .markdown-body .pl-en {
      color: var(--color-prettylights-syntax-entity);
    }

    .markdown-body .pl-smi,
    .markdown-body .pl-s .pl-s1 {
      color: var(--color-prettylights-syntax-storage-modifier-import);
    }

    .markdown-body .pl-ent {
      color: var(--color-prettylights-syntax-entity-tag);
    }

    .markdown-body .pl-k {
      color: var(--color-prettylights-syntax-keyword);
    }

    .markdown-body .pl-s,
    .markdown-body .pl-pds,
    .markdown-body .pl-s .pl-pse .pl-s1,
    .markdown-body .pl-sr,
    .markdown-body .pl-sr .pl-cce,
    .markdown-body .pl-sr .pl-sre,
    .markdown-body .pl-sr .pl-sra {
      color: var(--color-prettylights-syntax-string);
    }

    .markdown-body .pl-v,
    .markdown-body .pl-smw {
      color: var(--color-prettylights-syntax-variable);
    }

    .markdown-body .pl-bu {
      color: var(--color-prettylights-syntax-brackethighlighter-unmatched);
    }

    .markdown-body .pl-ii {
      color: var(--color-prettylights-syntax-invalid-illegal-text);
      background-color: var(--color-prettylights-syntax-invalid-illegal-bg);
    }

    .markdown-body .pl-c2 {
      color: var(--color-prettylights-syntax-carriage-return-text);
      background-color: var(--color-prettylights-syntax-carriage-return-bg);
    }

    .markdown-body .pl-sr .pl-cce {
      font-weight: bold;
      color: var(--color-prettylights-syntax-string-regexp);
    }

    .markdown-body .pl-ml {
      color: var(--color-prettylights-syntax-markup-list);
    }

    .markdown-body .pl-mh,
    .markdown-body .pl-mh .pl-en,
    .markdown-body .pl-ms {
      font-weight: bold;
      color: var(--color-prettylights-syntax-markup-heading);
    }

    .markdown-body .pl-mi {
      font-style: italic;
      color: var(--color-prettylights-syntax-markup-italic);
    }

    .markdown-body .pl-mb {
      font-weight: bold;
      color: var(--color-prettylights-syntax-markup-bold);
    }

    .markdown-body .pl-md {
      color: var(--color-prettylights-syntax-markup-deleted-text);
      background-color: var(--color-prettylights-syntax-markup-deleted-bg);
    }

    .markdown-body .pl-mi1 {
      color: var(--color-prettylights-syntax-markup-inserted-text);
      background-color: var(--color-prettylights-syntax-markup-inserted-bg);
    }

    .markdown-body .pl-mc {
      color: var(--color-prettylights-syntax-markup-changed-text);
      background-color: var(--color-prettylights-syntax-markup-changed-bg);
    }

    .markdown-body .pl-mi2 {
      color: var(--color-prettylights-syntax-markup-ignored-text);
      background-color: var(--color-prettylights-syntax-markup-ignored-bg);
    }

    .markdown-body .pl-mdr {
      font-weight: bold;
      color: var(--color-prettylights-syntax-meta-diff-range);
    }

    .markdown-body .pl-ba {
      color: var(--color-prettylights-syntax-brackethighlighter-angle);
    }

    .markdown-body .pl-sg {
      color: var(--color-prettylights-syntax-sublimelinter-gutter-mark);
    }

    .markdown-body .pl-corl {
      text-decoration: underline;
      color: var(--color-prettylights-syntax-constant-other-reference-link);
    }

    .markdown-body [role=button]:focus:not(:focus-visible),
    .markdown-body [role=tabpanel][tabindex="0"]:focus:not(:focus-visible),
    .markdown-body button:focus:not(:focus-visible),
    .markdown-body summary:focus:not(:focus-visible),
    .markdown-body a:focus:not(:focus-visible) {
      outline: none;
      box-shadow: none;
    }

    .markdown-body [tabindex="0"]:focus:not(:focus-visible),
    .markdown-body details-dialog:focus:not(:focus-visible) {
      outline: none;
    }

    .markdown-body g-emoji {
      display: inline-block;
      min-width: 1ch;
      font-family: "Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
      font-size: 1em;
      font-style: normal !important;
      font-weight: var(--base-text-weight-normal, 400);
      line-height: 1;
      vertical-align: -0.075em;
    }

    .markdown-body g-emoji img {
      width: 1em;
      height: 1em;
    }

    .markdown-body .task-list-item {
      list-style-type: none;
    }

    .markdown-body .task-list-item label {
      font-weight: var(--base-text-weight-normal, 400);
    }

    .markdown-body .task-list-item.enabled label {
      cursor: pointer;
    }

    .markdown-body .task-list-item+.task-list-item {
      margin-top: var(--base-size-4);
    }

    .markdown-body .task-list-item .handle {
      display: none;
    }

    .markdown-body .task-list-item-checkbox {
      margin: 0 .2em .25em -1.4em;
      vertical-align: middle;
    }

    .markdown-body ul:dir(rtl) .task-list-item-checkbox {
      margin: 0 -1.6em .25em .2em;
    }

    .markdown-body ol:dir(rtl) .task-list-item-checkbox {
      margin: 0 -1.6em .25em .2em;
    }

    .markdown-body .contains-task-list:hover .task-list-item-convert-container,
    .markdown-body .contains-task-list:focus-within .task-list-item-convert-container {
      display: block;
      width: auto;
      height: 24px;
      overflow: visible;
      clip: auto;
    }

    .markdown-body ::-webkit-calendar-picker-indicator {
      filter: invert(50%);
    }

    .markdown-body .markdown-alert {
      padding: var(--base-size-8) var(--base-size-16);
      margin-bottom: var(--base-size-16);
      color: inherit;
      border-left: .25em solid var(--borderColor-default);
    }

    .markdown-body .markdown-alert>:first-child {
      margin-top: 0;
    }

    .markdown-body .markdown-alert>:last-child {
      margin-bottom: 0;
    }

    .markdown-body .markdown-alert .markdown-alert-title {
      display: flex;
      font-weight: var(--base-text-weight-medium, 500);
      align-items: center;
      line-height: 1;
    }

    .markdown-body .markdown-alert.markdown-alert-note {
      border-left-color: var(--borderColor-accent-emphasis);
    }

    .markdown-body .markdown-alert.markdown-alert-note .markdown-alert-title {
      color: var(--fgColor-accent);
    }

    .markdown-body .markdown-alert.markdown-alert-important {
      border-left-color: var(--borderColor-done-emphasis);
    }

    .markdown-body .markdown-alert.markdown-alert-important .markdown-alert-title {
      color: var(--fgColor-done);
    }

    .markdown-body .markdown-alert.markdown-alert-warning {
      border-left-color: var(--borderColor-attention-emphasis);
    }

    .markdown-body .markdown-alert.markdown-alert-warning .markdown-alert-title {
      color: var(--fgColor-attention);
    }

    .markdown-body .markdown-alert.markdown-alert-tip {
      border-left-color: var(--borderColor-success-emphasis);
    }

    .markdown-body .markdown-alert.markdown-alert-tip .markdown-alert-title {
      color: var(--fgColor-success);
    }

    .markdown-body .markdown-alert.markdown-alert-caution {
      border-left-color: var(--borderColor-danger-emphasis);
    }

    .markdown-body .markdown-alert.markdown-alert-caution .markdown-alert-title {
      color: var(--fgColor-danger);
    }

    .markdown-body>*:first-child>.heading-element:first-child {
      margin-top: 0 !important;
    }

    .markdown-body .highlight pre:has(+zeroclipboard-container) {
      min-height: 52px;
    }
  `;
}
