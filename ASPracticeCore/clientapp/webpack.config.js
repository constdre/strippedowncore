const path = require("path");
const HtmlWebPackPlugin = require("html-webpack-plugin");

module.exports = {
    entry: {
        ShareableModule:"./src/ShareableModule.js",
        
    },
        
        
    output: {
        path: path.resolve(__dirname, "dist"),
        filename:"[name].js"
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/, //excludes node_modules folder from being transpiled
                use: {loader: "babel-loader"}

            },
            {
                test:/\.css$/,
                exclude: /node_modules/,
                use:["style-loader","css-loader"]
            }
            //{
            //    test: /\.html$/,
            //    use: [
            //        {
            //            loader:"html-loader" //will this work with cshtml as well?
            //        }
            //    ]
            //}

        ]
    }
    //plugins: [
    //    new HtmlWebPackPlugin({
    //        template: "./src/index.html",
    //    })
    //]
}