const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const postcssPresetEnv = require('postcss-preset-env');
const TerserPlugin = require("terser-webpack-plugin");

// We are getting 'process.env.NODE_ENV' from the NPM scripts
// Remember the 'dev' script?
const devMode = process.env.NODE_ENV !== 'production';
module.exports = {

    // Tells Webpack which built-in optimizations to use
    // If you leave this out, Webpack will default to 'production'
    mode: devMode ? 'development' : 'production',

    context: path.resolve(__dirname, 'wwwroot'),

    stats: { warnings: false },

    // Webpack needs to know where to start the bundling process,
    // so we define the Sass file under '/scss' directory
    // and the script file under '/js' directory
    entry: {
        './dist/js/site': './js/site.js',
        './dist/js/all': './js/all.js',
        './dist/css/site': ['./scss/site.scss']
    },
    // This is where we define the path where Webpack will place
    // a bundled JS file.
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot')
    },
    devtool: devMode ? 'inline-source-map' : 'source-map',
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            },
            {
                // Look for Sass files and process them according to the
                // rules specified in the different loaders
                test: /\.(sa|sc)ss$/,
                // Use the following loaders from right-to-left, so it will
                // use sass-loader first and ending with MiniCssExtractPlugin
                use: [
                    {
                        // Extracts the CSS into a separate file and uses the
                        // defined configurations in the 'plugins' section
                        loader: MiniCssExtractPlugin.loader
                    },
                    {
                        // Interprets CSS
                        loader: 'css-loader',
                        options: {
                            importLoaders: 2
                        }
                    },
                    {
                        // Use PostCSS to minify and autoprefix with vendor rules
                        // for older browser compatibility
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: {
                                ident: 'postcss',
                                // We instruct PostCSS to autoprefix and minimize our
                                // CSS when in production mode, otherwise don't do anything
                                plugins: devMode
                                    ? () => []
                                    : () => [
                                        postcssPresetEnv(),
                                        require('cssnano')()
                                    ]
                            }
                        }
                    },
                    {
                        // Adds support for Sass files, if using Less, then
                        // use the less-loader
                        loader: 'sass-loader'
                    }
                ]
            },
            {
                test: /\.(png|jpe?g|gif)$/,
                type: 'asset/resource',
                generator: {
                    filename: './assets/images/[name][ext]',
                }
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                type: 'asset/resource',
                generator: {
                    filename: './assets/fonts/[name][ext]',
                }
            }
        ]
    },
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin({
            extractComments: false
        })],
        
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: devMode ? './[name].css' : './minified/[name].min.css',
            chunkFilename: "[name].css"
        })
    ]
};

if (!devMode) {
    module.exports.devtool = '#source-map';
    
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        })
    ]);
}