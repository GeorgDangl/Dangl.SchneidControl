/*
 * FileSaver.js
 * A saveAs() FileSaver implementation.
 *
 * By Eli Grey, http://eligrey.com
 *
 * License : https://github.com/eligrey/FileSaver.js/blob/master/LICENSE.md (MIT)
 * source  : http://purl.eligrey.com/github/FileSaver.js
 */

// `a.click()` doesn't work for all browsers (#465)
function click(node: any) {
  try {
    node.dispatchEvent(new MouseEvent('click'));
  } catch (e) {
    var evt = document.createEvent('MouseEvents');
    evt.initMouseEvent(
      'click',
      true,
      true,
      window,
      0,
      0,
      0,
      80,
      20,
      false,
      false,
      false,
      false,
      0,
      null
    );
    node.dispatchEvent(evt);
  }
}

export function saveAs(blob: Blob, name: string) {
  // Namespace is used to prevent conflict w/ Chrome Poper Blocker extension (Issue #561)
  const a = document.createElementNS(
    'http://www.w3.org/1999/xhtml',
    'a'
  ) as any;
  name = name || (blob as any).name || 'download';

  a.download = name;
  a.rel = 'noopener'; // tabnabbing

  // Support blobs
  a.href = URL.createObjectURL(blob);
  setTimeout(function () {
    URL.revokeObjectURL(a.href);
  }, 4e4); // 40s
  setTimeout(function () {
    click(a);
  }, 0);
}
