# Merge PDF files

Merge the split PDF files in source directory to one PDF file.

## Usage

Usage of `merge` function.

```
PDFOrganizer: v1.0.0.0

Command 'merge' - Merge the PDF files in source directory

Usage:

merge --source <String> [--target <String>] [--prefix <String>] [--view <Boolean>]

Description:

    --source | --s      The source directory
    --target | --t      The target PDF file name
    --prefix | --p      The prefix filter in the source directory
    --view   | --v      Open the merged file after merge complete
```

## Example

To merge all the PDF files name start with ***pre*** under ***doc*** folder, and output to ***merge.pdf***, you can:

```
merge --s "doc" --t "merge" --p "pre" --v
```
