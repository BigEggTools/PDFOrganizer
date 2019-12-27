# Split PDF files

Split the source PDF files to one PDF file per page.

## Usage

Usage of `split` function.

```
PDFOrganizer: v1.0.0.0

Command 'split' - Split the source PDF file per page

Usage:

split --source <String> [--target <String>] [--prefix <String>]

Description:

    --source | --s      The source PDF file
    --target | --t      The target directory, same as source PDF file if not specific
    --prefix | --p      The prefix of the result files
```

## Example

To split the PDF file ***merge.pdf*** to ***doc*** folder, and all output PDF files' name start with ***pre***, you can:

```
split --s "merge.pdf" --t "doc" --p "pre"
```
